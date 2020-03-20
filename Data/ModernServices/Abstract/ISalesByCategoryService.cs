using System.Linq;
using Data.Entities;
using Data.ViewModels;

namespace Data.ModernServices.Abstract
{
    public interface ISalesByCategoryService
    {
        IQueryable<SaleByCategoryVM> SaleByCategory(IQueryable<SaleProduct> saleProducts,
            IQueryable<ProductInformation> productInformations);
    }
}