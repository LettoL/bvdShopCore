using System;
using System.Linq;
using Data;
using Data.Entities;
using Data.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebUI.ViewModels;

namespace WebUI.Controllers
{
    [Authorize]
    public class ShopController : Controller
    {
        private readonly IShopService _shopService;
        private readonly ISaleService _saleService;
        private readonly ShopContext _db;

        public ShopController(IShopService shopService, ISaleService saleService, ShopContext db)
        {
            _shopService = shopService;
            _saleService = saleService;
            _db = db;
        }

        public IActionResult Index()
        {
            return View(_shopService.All().ToList().Select(x => new ShopVM()
            {
                Id = x.Id,
                Title = x.Title,
                CashOnHand = _shopService.CashOnHand(_db, x.Id),
                Margin = _shopService.Margin(x.Id),
                Sales = _saleService.All()
                    .Where(s => s.Date.Month == DateTime.Now.AddHours(3).Month && s.Payment == true)
                    .Count(s => s.ShopId == x.Id),
                Turnover = _shopService.Turnover(x.Id)
            }));
        }

        [HttpGet]
        public IActionResult Get() 
        {
            var result = _shopService.All().ToList().Select(x => new ShopVM()
            {
                Id = x.Id,
                Title = x.Title,
            });

            return Ok(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Shop obj)
        {
            _shopService.Create(obj);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Shop shop = _shopService.All().First(u => u.Id == id);
            return View(shop);
        }

        [HttpPost]
        public IActionResult Edit(int id, string title)
        {
            _shopService.Update(new Shop()
            {
                Id = id,
                Title = title,
            });

            return RedirectToAction("Index", "Shop");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _shopService.Delete(id);

            return RedirectToAction("Index", "Shop");
        }
    }
}
