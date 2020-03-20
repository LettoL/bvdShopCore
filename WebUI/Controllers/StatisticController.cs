using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Data.Services.Abstract;
using Base.Services.Abstract;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using WebUI.ViewModels;

namespace WebUI.Controllers
{
    [Authorize]
    public class StatisticController : Controller
    {
        private readonly IBaseObjectService<Product> _productService;
        private readonly ISaleService _saleService;
        private readonly IBaseObjectService<SupplyProduct> _supplyProductService;

        public StatisticController(IBaseObjectService<Product> productService,
            ISaleService saleService,
            IBaseObjectService<SupplyProduct> supplyProductService)
        {
            _saleService = saleService;
            _productService = productService;
            _supplyProductService = supplyProductService;
        }

        public IActionResult Index()
        {
            ViewBag.Sum = _supplyProductService.All()
                .Sum(x => x.FinalCost * x.StockAmount);

            return View();
        }

        [HttpPost]
        public IActionResult GetTurnOverByMonth([FromBody] int month)
        {
            var s = _saleService.All().Where(i => i.Date.Month == month && i.Payment == true)
                .Select(x => new InfoMoneyVM
                {
                    Date = x.Date.ToString("dd.MM.yyyy"),
                    Sum = x.Sum
                });


            return Ok(_saleService.All().Where(i => i.Date.Month == month && i.Payment == true)
                .Select(x => new InfoMoneyVM
                {
                    Date = x.Date.ToString("dd.MM.yyyy"),
                    Sum = x.Sum
                }));

        }

        [HttpPost]
        public IActionResult GetTurnOverByYear([FromBody] int year)
        {
            return Ok(_saleService.All().Where(i => i.Date.Year == year && i.Payment == true)
                .Select(x => new InfoMoneyVM
                {
                    Date = x.Date.ToString("dd.MM.yyyy"),
                    Sum = x.Sum
                }));
        }
    }
}