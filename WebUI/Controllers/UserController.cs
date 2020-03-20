using System.Linq;
using Base.Services.Abstract;
using Data.Entities;
using Data.Enums;
using Data.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebUI.ViewModels;

namespace WebUI.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IBaseObjectService<User> _userService;
        private readonly IShopService _shopService;

        public UserController(IBaseObjectService<User> userService, IShopService shopService)
        {
            _userService = userService;
            _shopService = shopService;
        }

        public IActionResult Index()
        {
            return View(_userService.All().Select(x => new UserVM()
            {
                Id = x.Id,
                Title = x.Login,
                Role = x.Role,
                Password = x.Password,
                Shop = x.Shop
            }));
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.shops = _shopService.All();

            return View();
        }

        [HttpPost]
        public IActionResult Create(User obj)
        {
            _userService.Create(obj);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            User user = _userService.All().Select(x => new User()
            {
                Id = x.Id,
                Login = x.Login,
                Password = x.Password,
                Role = x.Role,
                Shop = x.Shop
            }).First(u => u.Id == id);

            ViewBag.shops = _shopService.All();

            return View(user);
        }

        [HttpPost]
        public IActionResult Edit(int id, string title, string password, Role role, int? shopId)
        {
            _userService.Update(new User()
            {
                Id = id,
                Login = title,
                Password = password,
                Role = role,
                ShopId = shopId
            });

            return RedirectToAction("Index", "User");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _userService.Delete(id);

            return RedirectToAction("Index", "User");
        }
    }
}
