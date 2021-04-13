using System;
using System.Linq;
using Base.Services.Abstract;
using Data;
using Data.Entities;
using Data.Services.Abstract;
using Domain.Entities.Olds;
using Microsoft.AspNetCore.Mvc;
using PostgresData;
using WebUI.ViewModels;

namespace WebUI.Controllers
{
    public class CardKeeperController : Controller
    {
        private IBaseObjectService<CardKeeper> _cardKeeperService { get; set; }
        private IInfoMoneyService _infoMoneyService { get; set; }

        private readonly ShopContext _db;
        private readonly PostgresContext _postgresContext;

        public CardKeeperController(IBaseObjectService<CardKeeper> cardKeeperService,
                IInfoMoneyService infoMoneyService,
                ShopContext db,
                PostgresContext postgresContext)
        {
            _cardKeeperService = cardKeeperService;
            _infoMoneyService = infoMoneyService;
            _db = db;
            _postgresContext = postgresContext;
        }

        public IActionResult Index()
        {
            var archiveCardKeepers = _postgresContext.ArchiveCardKeepers
                .Select(x => x.CardKeeperId).ToList();
            
            return View(_cardKeeperService.All()
                .Where(x => !archiveCardKeepers.Contains(x.Id))
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

        public IActionResult Delete(int id)
        {
            var cardKeeper = _db.CardKeepers.FirstOrDefault(x => x.Id == id);
            
            if(cardKeeper == null)
                throw new Exception("Держатель карты не существует");

            cardKeeper.ForManager = false;
            _db.CardKeepers.Update(cardKeeper);
            _db.SaveChanges();

            _postgresContext.ArchiveCardKeepers.Add(new ArchiveCardKeeper(id));
            _postgresContext.SaveChanges();
            
            return RedirectToAction("Index");
        }
    }
}