using System.Linq;
using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.API
{
    [ApiController]
    [Route("/api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ShopContext _db;

        public AuthController(ShopContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            var userName = HttpContext.User.Identity.Name;
            var user = _db.Users.FirstOrDefault(u => u.Login == userName);

            return Ok(new
            {
                UserId = user.Id,
                UserRole = user.Role.ToString()
            });
        }
    }
}