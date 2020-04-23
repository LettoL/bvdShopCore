using System;
using System.Threading.Tasks;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace WebUI.API
{
    [ApiController]
    [Route("/api/managers")]
    public class ManagersController : ControllerBase
    {
        private readonly IMongoDatabase db;

        public ManagersController()
        {
            var client = new MongoClient("mongodb+srv://admin:1234@cluster0-qpif1.azure.mongodb.net/test?retryWrites=true&w=majority");
            db = client.GetDatabase("bvdShop");
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await db.GetCollection<Manager>("managers")
               .Find(manager => true)
               .ToListAsync();
            
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string name)
        {
            await db.GetCollection<Manager>("managers")
                .InsertOneAsync(new Manager()
                {
                    Name = name,
                    CreationDate = DateTime.UtcNow
                });

            return Ok();
        }
    }
}