using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Domain.Entities;
using Domain.Entities.Sales;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostgresData;
using WebUI.Dtos;

namespace WebUI.API
{
    [ApiController]
    [Route("/api/managers")]
    public class ManagersController : ControllerBase
    {
        private readonly PostgresContext _postgresContext;
        private readonly ShopContext _db;
        public ManagersController(PostgresContext postgresContext, ShopContext db)
        {
            _postgresContext = postgresContext;
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var saleManagers = await _postgresContext.SaleManagersOld.ToListAsync();

            var salesId = saleManagers.Select(x => x.SaleId).ToList();
            var sales = await _db.Sales
                .Where(x => salesId.Contains(x.Id))
                .Select(x => new
                {
                    SaleId = x.Id,
                    Margin = x.Margin,
                    Sum = x.Sum,
                })
                .ToListAsync();

            var deletedManagersId = await _postgresContext.DeletedManagers
                .Select(x => x.Id)
                .ToListAsync();

            var managers = await _postgresContext.Managers
                .Where(x => !deletedManagersId.Contains(x.Id))
                .Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();

            var salesWithManager = sales.Select(x => new
            {
                SaleId = x.SaleId,
                Margin = x.Margin,
                Sum = x.Sum,
                ManagerId = saleManagers.FirstOrDefault(z => z.SaleId == x.SaleId).ManagerId
            }).ToList();

            var result = managers.Select(x => new ManagerDto()
            {
                Id = x.Id,
                Name = x.Name,
                Margin = salesWithManager
                    .Where(z => z.ManagerId == x.Id)
                    .Sum(z => z.Margin),
                Sum = salesWithManager
                    .Where(z => z.ManagerId == x.Id)
                    .Sum(z => z.Sum)
            }).ToList();
            
            return Ok(result);
        }

        [Route("GetList")]
        public async Task<IActionResult> GetList()
        {
            var deletedManagersId = await _postgresContext.DeletedManagers
                .Select(x => x.Id)
                .ToListAsync();

            var managers = await _postgresContext.Managers
                .Where(x => !deletedManagersId.Contains(x.Id))
                .Select(x => new ManagerDto
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();
            
            return Ok(managers);
        }

        [HttpPost]
        [Route("filter")]
        public async Task<IActionResult> Filter([FromBody] ManagerFilter query)
        {
            var saleManagers = await _postgresContext.SaleManagersOld.ToListAsync();

            var salesId = saleManagers.Select(x => x.SaleId).ToList();
            var sales = await _db.Sales
                .Where(x => salesId.Contains(x.Id))
                .Select(x => new
                {
                    SaleId = x.Id,
                    Margin = x.Margin,
                    Sum = x.Sum,
                    Date = x.Date
                })
                .ToListAsync();

            if (query.StartDate != null)
            {
                var date = DateTime.Parse(query.StartDate, new CultureInfo("ru-RU"));
                sales = sales.Where(x => x.Date.Date >= date.Date).ToList();
            }

            if (query.EndDate != null)
            {
                var date = DateTime.Parse(query.EndDate, new CultureInfo("ru-RU"));
                sales = sales.Where(x => x.Date.Date <= date.Date).ToList();
            }

            var managers = await _postgresContext.Managers
                .Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();

            var salesWithManager = sales.Select(x => new
            {
                SaleId = x.SaleId,
                Margin = x.Margin,
                Sum = x.Sum,
                ManagerId = saleManagers.FirstOrDefault(z => z.SaleId == x.SaleId).ManagerId
            });

            var result = managers.Select(x => new ManagerDto()
            {
                Id = x.Id,
                Name = x.Name,
                Margin = salesWithManager
                    .Where(z => z.ManagerId == x.Id)
                    .Sum(z => z.Margin),
                Sum = salesWithManager
                    .Where(z => z.ManagerId == x.Id)
                    .Sum(z => z.Sum)
            }).ToList();
            
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string name)
        {
            var result = await _postgresContext.Managers
                .AddAsync(new Manager(name));
            
            return Ok(result.Entity.Id);
        }
    }
}