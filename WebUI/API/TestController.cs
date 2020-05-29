using System.Threading.Tasks;
using Handlers.QueryHandler;
using Microsoft.AspNetCore.Mvc;
using PostgresData;

namespace WebUI.API
{
    [ApiController]
    [Route("/api/test")]
    public class TestController : ControllerBase
    {
        private readonly PostgresContext _db;

        public TestController(PostgresContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok("Yo");

        [HttpGet]
        [Route("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            var result = await GetProductHandler.Handle(_db);

            return Ok(result);
        }
    }
}