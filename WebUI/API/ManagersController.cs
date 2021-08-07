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
            var bookingManagers = await _postgresContext.BookingManagersOld.ToListAsync();

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

            var salesWithManager = sales.Join(saleManagers,
                s => s.SaleId,
                sm => sm.SaleId,
                (s, sm) => new
                {
                    SaleId = s.SaleId,
                    Margin = s.Margin,
                    Sum = s.Sum,
                    ManagerId = sm.ManagerId
                }).ToList();

            var salesWithManagerGroup = salesWithManager
                .GroupBy(x => x.ManagerId,
                    (managerId, sales) => new
                    {
                        ManagerId = managerId,
                        Sum = sales.Sum(x => x.Sum),
                        Margin = sales.Sum(x => x.Margin)
                    }).ToList();

            var infoMoneys = _db.InfoMonies
                .Where(x => x.SaleId != null || x.BookingId != null)
                .ToList();


            var bookingManagersWithSum = bookingManagers
                .Join(
                    infoMoneys.Where(x => x.BookingId != null && x.SaleId == null),
                    m => m.BookingId,
                    im => im.BookingId,
                    (m, im) => new
                    {
                        ManagerId = m.ManagerId,
                        Sum = im.Sum
                    })
                .GroupBy(
                    x => x.ManagerId,
                    (managerId, infos) => new
                    {
                        ManagerId = managerId,
                        Sum = infos.Sum(x => x.Sum)
                    })
                .ToList();

            var salesManagersWithSum = saleManagers
                .Join(
                    infoMoneys.Where(x => x.SaleId != null),
                    m => m.SaleId,
                    im => im.SaleId,
                    (m, im) => new
                    {
                        ManagerId = m.ManagerId,
                        Sum = im.Sum
                    })
                .GroupBy(
                    x => x.ManagerId,
                    (managerId, infos) => new
                    {
                        ManagerId = managerId,
                        Sum = infos.Sum(x => x.Sum)
                    })
                .ToList();

            var managersWithSum = salesManagersWithSum.Select(x => new
            {
                ManagerId = x.ManagerId,
                Sum = x.Sum + (bookingManagersWithSum
                    .FirstOrDefault(z => z.ManagerId == x.ManagerId)?
                    .Sum ?? 0)
            }).ToList();

           var result = managers
               .Join(salesWithManagerGroup,
                   m => m.Id,
                   s => s.ManagerId,
                   (m, s) => new ManagerDto()
                   {
                       Id = m.Id,
                       Name = m.Name,
                       Margin = s.Margin,
                       Sum = 0
                   })
               .Join(managersWithSum,
                   m => m.Id,
                   sm => sm.ManagerId,
                   (m, sm) => new ManagerDto()
                   {
                       Id = m.Id,
                       Name = m.Name,
                       Margin = m.Margin,
                       Sum = sm.Sum
                   })
               .ToList();
            
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
            var bookingManagers = await _postgresContext.BookingManagersOld.ToListAsync();

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

            var infoMoneys = await _db.InfoMonies
                .Where(x => x.SaleId != null || x.BookingId != null)
                .ToListAsync();

            if (query.StartDate != null)
            {
                var date = DateTime.Parse(query.StartDate, new CultureInfo("ru-RU"));
                sales = sales.Where(x => x.Date.Date >= date.Date).ToList();
                infoMoneys = infoMoneys.Where(x => x.Date.Date >= date.Date).ToList();
            }

            if (query.EndDate != null)
            {
                var date = DateTime.Parse(query.EndDate, new CultureInfo("ru-RU"));
                sales = sales.Where(x => x.Date.Date <= date.Date).ToList();
                infoMoneys = infoMoneys.Where(x => x.Date.Date <= date.Date).ToList();
            }

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

            var salesWithManager = sales.Join(saleManagers,
                s => s.SaleId,
                sm => sm.SaleId,
                (s, sm) => new
                {
                    SaleId = s.SaleId,
                    Margin = s.Margin,
                    Sum = s.Sum,
                    ManagerId = sm.ManagerId
                }).ToList();

            var salesWithManagerGroup = salesWithManager
                .GroupBy(x => x.ManagerId,
                    (managerId, sales) => new
                    {
                        ManagerId = managerId,
                        Sum = sales.Sum(x => x.Sum),
                        Margin = sales.Sum(x => x.Margin)
                    }).ToList();

            var bookingManagersWithSum = bookingManagers
                .Join(
                    infoMoneys.Where(x => x.BookingId != null && x.SaleId == null),
                    m => m.BookingId,
                    im => im.BookingId,
                    (m, im) => new
                    {
                        ManagerId = m.ManagerId,
                        Sum = im.Sum
                    })
                .GroupBy(
                    x => x.ManagerId,
                    (managerId, infos) => new
                    {
                        ManagerId = managerId,
                        Sum = infos.Sum(x => x.Sum)
                    })
                .ToList();

            var salesManagersWithSum = saleManagers
                .Join(
                    infoMoneys.Where(x => x.SaleId != null),
                    m => m.SaleId,
                    im => im.SaleId,
                    (m, im) => new
                    {
                        ManagerId = m.ManagerId,
                        Sum = im.Sum
                    })
                .GroupBy(
                    x => x.ManagerId,
                    (managerId, infos) => new
                    {
                        ManagerId = managerId,
                        Sum = infos.Sum(x => x.Sum)
                    })
                .ToList();

            var managersWithSum = salesManagersWithSum.Select(x => new
            {
                ManagerId = x.ManagerId,
                Sum = x.Sum + (bookingManagersWithSum
                    .FirstOrDefault(z => z.ManagerId == x.ManagerId)?
                    .Sum ?? 0)
            }).ToList();

           var result = managers
               .Join(salesWithManagerGroup,
                   m => m.Id,
                   s => s.ManagerId,
                   (m, s) => new ManagerDto()
                   {
                       Id = m.Id,
                       Name = m.Name,
                       Margin = s.Margin,
                       Sum = 0
                   })
               .Join(managersWithSum,
                   m => m.Id,
                   sm => sm.ManagerId,
                   (m, sm) => new ManagerDto()
                   {
                       Id = m.Id,
                       Name = m.Name,
                       Margin = m.Margin,
                       Sum = sm.Sum
                   })
               .ToList();
            
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