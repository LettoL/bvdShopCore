using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Data;
using Data.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly ShopContext _db;

        public AccountController(ShopContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string login, string password)
        {
            Console.WriteLine(1);
            
            try
            {
                User user = await _db.Users
                    .FirstOrDefaultAsync(u => u.Login == login && u.Password == password);

                Console.WriteLine(2);
                
                if (user != null)
                {
                    Console.WriteLine(3);
                    
                    await Authenticate(login); // аутентификация

                    Console.WriteLine(4);
                    
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return RedirectToAction("Index", "Account");
        }

        private async Task Authenticate(string userName)
        {
            // создаем один claim
            var claims = new List<Claim>

            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}