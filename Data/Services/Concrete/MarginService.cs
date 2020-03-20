using Data.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Services.Concrete
{
    public class MarginService : IMarginService
    {
        private readonly ITurnOverService _turnOverService;
        private readonly ISaleService _saleService;
        private readonly IPrimeCostService _primeCostService;

        public MarginService(ITurnOverService turnOverService,
            ISaleService saleService,
            IPrimeCostService primeCostService)
        {
            _turnOverService = turnOverService;
            _saleService = saleService;
            _primeCostService = primeCostService;
        }

        public decimal GetMarginShop(int shopId, int categoryId)
        {
            var turnOver = _turnOverService.GetTurnOverShop(shopId, categoryId);

            var primeCost = _primeCostService.GetPrimeCostShop(shopId, categoryId);

            return turnOver - primeCost;
        }

        public decimal GetClearMarginShop(int shopId, int categoryId)
        {
            var turnOver = _turnOverService.GetClearTurnOverShop(shopId, categoryId);

            var primeCost = _primeCostService.GetClearPrimeCostShop(shopId, categoryId);

            return turnOver - primeCost;
        }

        public decimal GetMarginPartners(int categoryId)
        {
            var turnOver = _turnOverService.GetTurnOverPartners(categoryId);

            var primeCost = _primeCostService.GetPrimeCostPartners(categoryId);  

            return turnOver - primeCost;
        }

        public decimal GetMarginRussian(int categoryId)
        {
            var turnOver = _turnOverService.GetTurnOverRussian(categoryId);

            var primeCost = _primeCostService.GetPrimeCostRussian(categoryId);

            return turnOver - primeCost;
        }

    }
}
