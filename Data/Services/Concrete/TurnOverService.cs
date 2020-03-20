using Base.Services.Abstract;
using Data.Entities;
using Data.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Services.Concrete
{
    public class TurnOverService : ITurnOverService
    {
        private readonly ISaleService _saleService;
        private readonly IBaseObjectService<SaleProduct> _saleProductService;

        public TurnOverService(
            ISaleService saleService,
            IBaseObjectService<SaleProduct> saleProductService)
        {
            _saleService = saleService;
            _saleProductService = saleProductService;
        }

        public decimal GetTurnOverShop(int shopId, int categoryId)
        {
            var saleIds = _saleService.GetSalesByShop(shopId)
                .Select(x => x.Id);

            return GetTurnOver(saleIds, categoryId);
        }

        public decimal GetClearTurnOverShop(int shopId, int categoryId)
        {
            var saleIds = _saleService
                .GetSalesByShop(shopId)
                .Where(x => x.PartnerId == null && x.ForRussian == false)
                .Select(x => x.Id);

            return GetTurnOver(saleIds, categoryId);
        }

        public decimal GetTurnOverPartners(int categoryId)
        {
            var saleIds = _saleService.GetSalesByPartners()
                .Select(x => x.Id);

            return GetTurnOver(saleIds, categoryId);
        }

        public decimal GetTurnOverRussian(int categoryId)
        {
            var saleIds = _saleService.GetSalesByRussian()
                .Select(x => x.Id);

            return GetTurnOver(saleIds, categoryId);
        }

        private decimal GetTurnOver(IQueryable<int> saleIds, int categoryId)
        {
            var saleProducts = _saleProductService.All()
                .Where(x => saleIds.Contains(x.SaleId) && x.Product.CategoryId == categoryId)
                .Select(x => new SaleProduct
                {
                    Id = x.Id,
                    Amount = x.Amount,
                    Additional = x.Additional,
                    ProductId = x.ProductId,
                    SaleId = x.SaleId,
                    Cost = x.Cost * x.Amount - (x.Sale.Discount / x.Sale.SalesProducts.Sum(z => z.Amount))
                }).ToList();


            var result = saleProducts.Sum(x => x.Cost * x.Amount);

            return Math.Round(result, 2);
        }
    }
}
