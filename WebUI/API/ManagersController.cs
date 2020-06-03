using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostgresData;

namespace WebUI.API
{
    [ApiController]
    [Route("/api/managers")]
    public class ManagersController : ControllerBase
    {
        private readonly PostgresContext _postgresContext;
        public ManagersController(PostgresContext postgresContext)
        {
            _postgresContext = postgresContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _postgresContext.Managers
                .Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListAsync();
            
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