using Base.Services.Abstract;
using Data.Entities;
using Data.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Services.Concrete
{
    public class PrimeCostService : IPrimeCostService
    {
        private readonly ISaleService _saleService;
        private readonly IBaseObjectService<ProductInformation> _productInformationService;

        public PrimeCostService(ISaleService saleService,
            IBaseObjectService<ProductInformation> productInformationService)
        {
            _saleService = saleService;
            _productInformationService = productInformationService;
        }

        public decimal GetPrimeCostShop(int shopId, int categoryId)
        {
            var salesIds = _saleService.GetSalesByShop(shopId)
                .Select(x => x.Id);

            return GetPrimeCost(salesIds, categoryId);
        }

        public decimal GetClearPrimeCostShop(int shopId, int categoryId)
        {
            var salesIds = _saleService.GetSalesByShop(shopId)
                .Where(x => x.PartnerId == null && x.ForRussian == false)
                .Select(x => x.Id);

            return GetPrimeCost(salesIds, categoryId);
        }

        public decimal GetPrimeCostPartners(int categoryId)
        {
            var saleIds = _saleService.GetSalesByPartners()
                .Select(x => x.Id);

            return GetPrimeCost(saleIds, categoryId);
        }

        public decimal GetPrimeCostRussian(int categoryId)
        {
            var saleIds = _saleService.GetSalesByRussian()
                .Select(x => x.Id);

            return GetPrimeCost(saleIds, categoryId);
        }

        private decimal GetPrimeCost(IQueryable<int> salesIds, int categoryId)
        {
            var productsInformations = _productInformationService.All()
               .Where(x => salesIds.Contains(x.SaleId) && x.Product.CategoryId == categoryId);
            
            var result = productsInformations.Sum(x => x.FinalCost);

            return result;
        }
    }
}
