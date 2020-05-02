using System;
using System.Linq;
using Data;
using Data.Enums;
using Microsoft.AspNetCore.Mvc;
using WebUI.Dtos;

namespace WebUI.API
{
    [ApiController]
    [Route("/api/productsHistory")]
    public class ProductsHistory : ControllerBase
    {
        private readonly ShopContext _db;

        public ProductsHistory(ShopContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var result = _db.InfoProducts
                    .OrderByDescending(x => x.Id)
                    .Select(x => new ProductsHistoryItem()
                    {
                        Id = x.Id,
                        DateTime = x.Date,
                        ProductTitle = x.Product.Title,
                        Amount = x.Amount,
                        SupplierName = x.Supplier.Title,
                        Type = x.Type == InfoProductType.Supply
                            ? "Поставка"
                            : x.Type == InfoProductType.Writeoff
                                ? "Списание"
                                : "Перенос",
                        ShopTitle = x.Shop.Title,
                        ShopId = x.ShopId
                    }).ToList();
                
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}