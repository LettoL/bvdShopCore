﻿using System.Linq;
using Base.Services.Abstract;
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

        public PartnerController(IBaseObjectService<Partner> partnerService,
            ISaleService saleService)
        {
            _partnerService = partnerService;
            _saleService = saleService;
        }

        public IActionResult Index()
        {
            return View(_partnerService.All().Select(x => new PartnerVM()
            {
                Id = x.Id,
                BuyProductsAmount = _saleService.All()
                    .Where(s => s.PartnerId == x.Id).Sum(s => s.SalesProducts
                        .Sum(sp => sp.Amount)),
                Email = x.Email,
                Phone = x.Phone,
                Title = x.Title
            }));
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
