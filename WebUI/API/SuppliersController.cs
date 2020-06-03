using System;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;
using PostgresData;
using WebUI.Commands;

namespace WebUI.API
{
    [ApiController]
    [Route("/api/suppliers")]
    public class SuppliersController : ControllerBase
    {
        private readonly ShopContext _db;
        private readonly PostgresContext _postgresContext;

        public SuppliersController(ShopContext db, PostgresContext postgresContext)
        {
            _db = db;
            _postgresContext = postgresContext;
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

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SupplierCreate command)
        {
            try
            {
                var createTask = await _db.Suppliers.AddAsync(new Supplier()
                {
                    Title = command.Name,
                    Phone = command.Phone,
                    Email = command.Email,
                    Debt = 0
                });

                await _db.SaveChangesAsync();


                await _postgresContext.Suppliers.AddAsync(
                    new Domain.Entities.Supplier(command.Name, command.Phone, command.Email));
                await _postgresContext.SaveChangesAsync();

                return Ok(new {Id = createTask.Entity.Id, Name = createTask.Entity.Title});
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}