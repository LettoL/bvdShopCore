using System;
using System.Threading.Tasks;
using Domain.Commands;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostgresData;

namespace WebUI.API
{
    [ApiController]
    [Route("/api/incompletesProducts")]
    public class IncompleteProducts : ControllerBase
    {
        private readonly PostgresContext _postgresContext;

        public IncompleteProducts(PostgresContext postgresContext)
        {
            _postgresContext = postgresContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _postgresContext.IncompleteProducts.ToListAsync();

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

       /* [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateIncompleteProduct command)
        {
            try
            {
                var createdEntity = await _postgresContext.IncompleteProducts
                    .AddAsync(new IncompleteProduct(
                        command.ProductId,
                        command.Amount,
                        command.ShopId,
                        command.Comment));

                await _postgresContext.SaveChangesAsync();

                return Ok(createdEntity.Entity.Id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }*/
    }
}