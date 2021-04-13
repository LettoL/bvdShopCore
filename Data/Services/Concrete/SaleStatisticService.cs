using Base.Services.Abstract;
using Data.Entities;
using Data.FiltrationModels;
using Data.Services.Abstract;
using Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Data.Services.Concrete
{
    public class SaleStatisticService : ISaleStatisticService
    {
        private readonly IBaseObjectService<Category> _categoryService;
        private readonly ISalesAmountService _saleAmountService;
        private readonly ITurnOverService _turnOverService;
        private readonly IMarginService _marginService;
        private readonly ISaleInfoService _saleInfoService;
        private readonly ISaleService _saleService;
        private readonly IBaseObjectService<InfoMoney> _infoMoneyService;

        public SaleStatisticService(IBaseObjectService<Category> categoryService,
            ISalesAmountService saleAmountService,
            ITurnOverService turnOverService,
            IMarginService marginService,
            ISaleInfoService saleInfoService,
            ISaleService saleService,
            IBaseObjectService<InfoMoney> infoMoneyService)
        {
            _categoryService = categoryService;
            _saleAmountService = saleAmountService;
            _turnOverService = turnOverService;
            _marginService = marginService;
            _saleInfoService = saleInfoService;
            _saleService = saleService;
            _infoMoneyService = infoMoneyService;
        }

        public IQueryable<SaleByCategoryVM> GetSalesByCategoriesStats()
        {
            var moscowShopId = 1;
            var petersburgShopId = 2;

            var saleCategories = _categoryService.All()
                .Select(x => new SaleByCategoryVM
                {
                    Category = x,
                    SalesByMoscow = _saleAmountService.GetClearSalesAmountShop(moscowShopId, x.Id),
                    SalesByPetersburg = _saleAmountService.GetClearSalesAmountShop(petersburgShopId, x.Id),
                    PartnerSales = _saleAmountService.GetSalesAmountPartners(x.Id),
                    ForRussianFederation = _saleAmountService.GetSalesAmountRussian(x.Id),

                    TurnOverMoscow = _turnOverService.GetClearTurnOverShop(moscowShopId, x.Id),
                    TurnOverPetersburg = _turnOverService.GetClearTurnOverShop(petersburgShopId, x.Id),
                    TurnOverPartner = _turnOverService.GetTurnOverPartners(x.Id),
                    TurnOverRF = _turnOverService.GetTurnOverRussian(x.Id),
                    TurnOver = _turnOverService.GetClearTurnOverShop(moscowShopId, x.Id) +
                        _turnOverService.GetClearTurnOverShop(petersburgShopId, x.Id) + 
                        _turnOverService.GetTurnOverPartners(x.Id) +
                        _turnOverService.GetTurnOverRussian(x.Id),

                    MarginMoscow = _marginService.GetClearMarginShop(moscowShopId, x.Id),
                    MarginPetersburg = _marginService.GetClearMarginShop(petersburgShopId, x.Id),
                    MarginPartner = _marginService.GetMarginPartners(x.Id),
                    MarginRF = _marginService.GetMarginRussian(x.Id),
                    Margin = _marginService.GetClearMarginShop(moscowShopId, x.Id) +
                        _marginService.GetClearMarginShop(petersburgShopId, x.Id) +
                        _marginService.GetMarginPartners(x.Id) +
                        _marginService.GetMarginRussian(x.Id)
                });

            return saleCategories;
        }

        public IQueryable<SaleListVM> SaleList(ShopContext db)
        {
            var infoMoneys = db.InfoMonies.ToList();
            
            var query = db.Sales
                .Where(x => x.Payment)
                .Include(x => x.SalesProducts).ThenInclude(x => x.Product)
                .Include(x => x.Partner)
                .OrderByDescending(x => x.Date)
                .Take(100)
                .Select(x => new SaleListVM()
                {
                    Id = x.Id,
                    Date = x.Date.ToString("dd.MM.yyyy"),
                    Sum = db.InfoMonies.Where(z => z.SaleId == x.Id).Sum(z => z.Sum),
                    ShopTitle = x.Shop.Title,
                    PrimeCost = x.PrimeCost,
                    ProductTitle = x.SalesProducts.FirstOrDefault().Product.Title,
                    BuyerTitle = x.PartnerId != null
                        ? x.Partner.Title
                        : "Обычный покупатель",
                    HasAdditionalProduct = x.SalesProducts.Any(x => x.Additional),
                    MarginPercent = Math.Round((x.Margin == 0 || x.Sum  == 0) ? 0 : x.Margin/(x.Sum/100))
                })
                .ToList()
                .Select(x => new SaleListVM()
                {
                    Id = x.Id,
                    Date = x.Date,
                    Sum = x.Sum,
                    PrimeCost = x.PrimeCost,
                    ShopTitle = x.ShopTitle,
                    HasAdditionalProduct = x.HasAdditionalProduct,
                    BuyerTitle = x.BuyerTitle,
                    ProductTitle = x.ProductTitle,
                    PaymentType = _saleInfoService.PaymentType(x.Id, infoMoneys),
                    MarginPercent = Math.Round(x.MarginPercent)
                }).AsQueryable();

            return query;
        }
    }
}
