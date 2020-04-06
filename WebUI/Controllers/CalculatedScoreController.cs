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
    public class CalculatedScoreController : Controller
    {
        private readonly IBaseObjectService<CalculatedScore> _calculatedScoreService;
        private readonly IInfoMoneyService _infoMoneyService;
        private readonly ShopContext _db;

        public CalculatedScoreController(IBaseObjectService<CalculatedScore> calculatedScoreService,
            IInfoMoneyService infoMoneyService,
            ShopContext db)
        {
            _calculatedScoreService = calculatedScoreService;
            _infoMoneyService = infoMoneyService;
            _db = db;
        }

        public IActionResult Index()
        {
            return View(_calculatedScoreService.All()
                .Select(cs => new CalculatedScoreVM()
                {
                    Id = cs.Id,
                    Title = cs.Title,
                    Balance = _infoMoneyService.GetMoneyWorkerBalance(_db, cs.Id)
                })
            );
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CalculatedScore calculatedScore)
        {
            _calculatedScoreService.Create(calculatedScore);

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            return View(_calculatedScoreService.All().FirstOrDefault(cs => cs.Id == id));
        }

        [HttpPost]
        public IActionResult Edit(CalculatedScore calculatedScore)
        {
            _calculatedScoreService.Update(calculatedScore);

            return RedirectToAction("Index");
        }
    }
}