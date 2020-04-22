using System.Linq;
using Base.Services.Abstract;
using Data;
using Data.Entities;
using Data.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebUI.ViewModels;

namespace WebUI.Controllers
{
    [Authorize]
    public class PartnerController : Controller
    {
        private IBaseObjectService<Partner> _partnerService;
        private ISaleService _saleService;
        private readonly ShopContext _db;

        public PartnerController(IBaseObjectService<Partner> partnerService,
            ISaleService saleService,
            ShopContext db)
        {
            _partnerService = partnerService;
            _saleService = saleService;
            _db = db;
        }

        public IActionResult Index()
        {
            var saleProductsAmount = _db.SalesProducts
                .Where(x => x.Sale.PartnerId != null && x.Sale.PartnerId != 0)
                .Select(x => new
                {
                    PartnerId = x.Sale.PartnerId,
                    Amount = x.Amount
                });

            var result = _db.Partners.Select(x => new PartnerVM()
            {
                Id = x.Id,
                BuyProductsAmount = saleProductsAmount
                    .Where(s => s.PartnerId == x.Id)
                    .Sum(s => s.Amount),
                Email = x.Email,
                Phone = x.Phone,
                Title = x.Title
            }).ToList();
            
            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Partner obj)
        {
            _partnerService.Create(obj);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            return View(_partnerService.Get(id));
        }

        [HttpPost]
        public IActionResult Edit(Partner obj)
        {
            _partnerService.Update(obj);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _partnerService.Delete(id);

            return RedirectToAction("Index");
        }
    }
}
