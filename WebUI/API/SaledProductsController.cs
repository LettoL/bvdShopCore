using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;
using WebUI.ViewModels;

namespace WebUI.API
{
    [ApiController]
    [Route("/api/saledProducts")]
    public class SaledProductsController : ControllerBase
    {
        private readonly ShopContext _shopContext;

        public SaledProductsController(ShopContext shopContext)
        {
            _shopContext = shopContext;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var currentDate = DateTime.Now.AddHours(3).Date;

            var productInformationsByCurrentDay = _shopContext.ProductInformations
                .Where(x => x.Sale.Date.Date == currentDate);

            var result = GetSoldProducts(productInformationsByCurrentDay);

            return Ok(result);
        }

        [HttpPost("byDates")]
        public IActionResult GetByDates([FromBody] DatesFilter datesFilter)
        {
            DateTime? from = null;
            DateTime? to = null;

            if (datesFilter.From != null)
            {
                var buf = datesFilter.From.Split('-');
                from = new DateTime(
                    Convert.ToInt32(buf[0]),
                    Convert.ToInt32(buf[1]),
                    Convert.ToInt32(buf[2]));
            }
            
            if (datesFilter.To != null)
            {
                var buf = datesFilter.To.Split('-');
                to = new DateTime(
                    Convert.ToInt32(buf[0]),
                    Convert.ToInt32(buf[1]),
                    Convert.ToInt32(buf[2]));
            }

            var productInformationsByDates = _shopContext.ProductInformations.AsQueryable();

            if (from != null)
                productInformationsByDates = productInformationsByDates.Where(x => x.Sale.Date.Date >= from);

            if (to != null)
                productInformationsByDates = productInformationsByDates.Where(x => x.Sale.Date.Date <= to);

            var result = GetSoldProducts(productInformationsByDates);

            return Ok(result);
        }

        private ICollection<SaledProductsVM> GetSoldProducts(IQueryable<ProductInformation> query)
        {
            var result = query.Select(x => new
                {
                    ProductId = x.ProductId,
                    Amount = x.Amount,
                    CategoryId = x.Product.CategoryId,
                    ShopId = x.Sale.ShopId,
                    ShopTitle = x.Sale.Shop.Title,
                    SupplierId = x.SupplyProduct.SupplierId,
                    Title = x.Product.Title,
                }).ToList()
                .Where(x => x.ProductId != null)
                .GroupBy(x => new
                {
                    x.ProductId,
                    x.ShopId
                })
                .Select(x => new SaledProductsVM()
                {
                    ProductId = (int)x.Key.ProductId,
                    Amount = x.Sum(z => z.Amount),
                    CategoryId = x.FirstOrDefault().CategoryId,
                    ShopId = x.Key.ShopId,
                    SuppliersId = x.Where(z => z.SupplierId != null)
                        .Select(z => (int)z.SupplierId).ToList(),
                    Title = x.FirstOrDefault().Title,
                    ShopTitle = x.FirstOrDefault().ShopTitle
                })
                .ToList();

           return result;
        }
    }

    public class DatesFilter
    {
        public string From { get; set; }
        public string To { get; set; }
    }
}