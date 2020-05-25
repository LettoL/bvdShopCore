using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.API
{
    [ApiController]
    [Route("/api/test")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get() => Ok("Yo");
    }
}