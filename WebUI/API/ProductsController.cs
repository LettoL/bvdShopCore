using System;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebUI.Dtos;

namespace WebUI.API
{
    [ApiController]
    [Route("/api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly ShopContext _db;

        public ProductsController(ShopContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var result = ProductService.GetAllProducts(_db)
                    .Select(x => new ProductsAdmin()
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Title = x.Title,
                        Amount = x.Amount,
                        Price = x.Cost,
                        Shop = x.Shop.Title,
                        ShopId = x.Shop.Id,
                        Category = x.Category.Title,
                        CategoryId = x.Category.Id
                    });

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}