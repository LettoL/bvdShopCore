using System.Linq;
using Data;
using Data.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ShopContext _db;

        public HomeController(ShopContext db)
        {
            _db = db;
        }

        // GET: /<controller>/
        [Authorize]
        public IActionResult Index()
        {
            var userName = HttpContext.User.Identity.Name;
            var user = _db.Users.FirstOrDefault(x => x.Login == userName);

            if (user.Role == Role.Administrator)
                return RedirectToAction("Index", "Admin");

            if (user.Role == Role.Manager)
                return RedirectToAction("Index", "Manager");

            return View();
        }
    }
}
