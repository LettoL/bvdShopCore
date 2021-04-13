using System;
using System.Linq;
using Base.Services.Abstract;
using Data;
using Data.Entities;
using Data.Services.Abstract;
using Domain.Entities.Olds;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostgresData;
using WebUI.ViewModels;

namespace WebUI.Controllers
{
    [Authorize]
    public class CalculatedScoreController : Controller
    {
        private readonly IBaseObjectService<CalculatedScore> _calculatedScoreService;
        private readonly IInfoMoneyService _infoMoneyService;
        private readonly ShopContext _db;
        private readonly PostgresContext _postgresContext;

        public CalculatedScoreController(IBaseObjectService<CalculatedScore> calculatedScoreService,
            IInfoMoneyService infoMoneyService,
            ShopContext db,
            PostgresContext postgresContext)
        {
            _calculatedScoreService = calculatedScoreService;
            _infoMoneyService = infoMoneyService;
            _db = db;
            _postgresContext = postgresContext;
        }

        public IActionResult Index()
        {
            var archiveCalculatedScores = _postgresContext.ArchiveCalculatedScores
                .Select(x => x.CalculatedScoreId).ToList();
            
            return View(_calculatedScoreService.All()
                .Where(x => !archiveCalculatedScores.Contains(x.Id))
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

        public IActionResult Delete(int id)
        {
            var calculatedScore = _db.CalculatedScores.FirstOrDefault(x => x.Id == id);
            
            if(calculatedScore == null)
                throw new Exception("Рассчётный счёт не найден");

            _postgresContext.ArchiveCalculatedScores.Add(new ArchiveCalculatedScore(id));
            _postgresContext.SaveChanges();
            
            return RedirectToAction("Index");
        }
    }
}