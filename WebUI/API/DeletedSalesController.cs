using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PostgresData;

namespace WebUI.API
{
    [ApiController]
    [Route("/api/deletedSales")]
    public class DeletedSalesController : ControllerBase
    {
        private readonly PostgresContext _postgresContext;

        public DeletedSalesController(PostgresContext postgresContext)
        {
            _postgresContext = postgresContext;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var result = _postgresContext.DeletedSalesInfoOld
                    .Select(x => x.Sale)
                    .OrderByDescending(x => x.Number)
                    .ToList();

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}