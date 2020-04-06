using System.Linq;
using Base.Services.Abstract;
using Data;
using Data.Entities;
using Data.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using WebUI.ViewModels;

namespace WebUI.Controllers
{
    public class CardKeeperController : Controller
    {
        private IBaseObjectService<CardKeeper> _cardKeeperService { get; set; }
        private IInfoMoneyService _infoMoneyService { get; set; }

        private readonly ShopContext _db;

        public CardKeeperController(IBaseObjectService<CardKeeper> cardKeeperService,
                IInfoMoneyService infoMoneyService,
                ShopContext db)
        {
            _cardKeeperService = cardKeeperService;
            _infoMoneyService = infoMoneyService;
            _db = db;
        }

        public IActionResult Index()
        {
            return View(_cardKeeperService.All()
                .Select(ck => new CardKeeperVM()
                {
                    Id = ck.Id,
                    Title =  ck.Title,
                    CardNumber = ck.CardNumber,
                    ForManager = ck.ForManager,
                    Balance = _infoMoneyService.GetMoneyWorkerBalance(_db, ck.Id)
                })
            );
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CardKeeper cardKeeper)
        {
            _cardKeeperService.Create(cardKeeper);

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            return View(_cardKeeperService.All().FirstOrDefault(ck => ck.Id == id));
        }

        [HttpPost]
        public IActionResult Edit(CardKeeper cardKeeper)
        {
            _cardKeeperService.Update(cardKeeper);
            return RedirectToAction("Index");
        }
    }
}