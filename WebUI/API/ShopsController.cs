using System;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Entities;
using Handlers.CommandHandlers;
using Handlers.Commands;
using Microsoft.AspNetCore.Mvc;
using PostgresData;
using WebUI.Commands;
using WebUI.Dtos;

namespace WebUI.API
{
    [ApiController]
    [Route("/api/shops")]
    public class ShopsController : ControllerBase
    {
        private readonly ShopContext _db;
        private readonly PostgresContext _postgresContext;

        public ShopsController(ShopContext db, PostgresContext postgresContext)
        {
            _db = db;
            _postgresContext = postgresContext;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var result = _db.Shops
                    .Select(x => new ShopItemFilter()
                    {
                        Id = x.Id,
                        Title = x.Title
                    }).ToList();

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ShopCreate command)
        {
            try
            {
                var shopCreateTask = await _db.Shops.AddAsync(new Shop()
                {
                    Title = command.Title
                });

                await _db.SaveChangesAsync();

                await command.Handler(_postgresContext);

                var result = shopCreateTask.Entity;
                
                return Ok(new {id = result.Id, title = command.Title});
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}