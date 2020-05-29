using System;
using System.Threading.Tasks;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.API
{
    [ApiController]
    [Route("/api/managers")]
    public class ManagersController : ControllerBase
    {

        public ManagersController()
        {
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok("managers get");
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string name)
        {
            return Ok("managers post");
        }
    }
}