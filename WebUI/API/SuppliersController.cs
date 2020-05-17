using System.Linq;
using Data;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.API
{
    [ApiController]
    [Route("/api/suppliers")]
    public class SuppliersController : ControllerBase
    {
        private readonly ShopContext _db;

        public SuppliersController(ShopContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = _db.Suppliers
                .Select(x => new
                {
                    Id = x.Id,
                    Name = x.Title
                }).ToList();

            return Ok(result);
        }
    }
}