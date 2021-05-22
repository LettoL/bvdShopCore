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
            var productInformationsByDates = _shopContext.ProductInformations
                .Where(x => x.Sale.Date.Date >= datesFilter.From.Date
                            && x.Sale.Date.Date <= datesFilter.To.Date);

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
                .GroupBy(x => x.ProductId)
                .Select(x => new SaledProductsVM()
                {
                    ProductId = (int)x.Key,
                    Amount = x.Sum(z => z.Amount),
                    CategoryId = x.FirstOrDefault().CategoryId,
                    ShopId = x.FirstOrDefault().ShopId,
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
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}