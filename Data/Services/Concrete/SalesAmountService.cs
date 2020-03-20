using Base.Services.Abstract;
using Data.Entities;
using Data.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Services.Concrete
{
    public class SalesAmountService : ISalesAmountService
    {
        private readonly ISaleService _saleService;
        private readonly IBaseObjectService<SaleProduct> _saleProductService;

        public SalesAmountService(
            ISaleService saleService,
            IBaseObjectService<SaleProduct> saleProductService)
        {
            _saleService = saleService;
            _saleProductService = saleProductService;
        }

        public int GetSalesAmountShop(int shopId, int categoryId)
        {
            var salesIds = _saleService.GetSalesByShop(shopId)
                .Select(x => x.Id);

            return GetSalesAmount(salesIds, categoryId);
        }

        public int GetClearSalesAmountShop(int shopId, int categoryId)
        {
            var salesIds = _saleService.GetSalesByShop(shopId)
                .Where(x => x.PartnerId == null && x.ForRussian == false)
                .Select(x => x.Id);

            return GetSalesAmount(salesIds, categoryId);
        }

        public int GetSalesAmountPartners(int categoryId)
        {
            var salesIds = _saleService.GetSalesByPartners()
                .Select(x => x.Id);

            return GetSalesAmount(salesIds, categoryId);
        }

        public int GetSalesAmountRussian(int categoryId)
        {
            var salesIds = _saleService.GetSalesByRussian()
                .Select(x => x.Id);           

            return GetSalesAmount(salesIds, categoryId);
        }

        private int GetSalesAmount(IQueryable<int> salesIds, int categoryId)
        {
            var salesProducts = _saleProductService.All()
                .Where(x => salesIds.Contains(x.SaleId) && x.Product.CategoryId == categoryId);

            var result = salesProducts.Sum(x => x.Amount);

            return result;
        }
    }
}
