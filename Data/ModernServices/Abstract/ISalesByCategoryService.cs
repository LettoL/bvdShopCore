using System.Collections.Generic;
using System.Linq;
using Data.Entities;
using Data.ViewModels;

namespace Data.ModernServices.Abstract
{
    public interface ISalesByCategoryService
    {
        ICollection<SaleByCategoryVM> SaleByCategory(ShopContext db, ICollection<SaleProduct> saleProducts,
            ICollection<ProductInformation> productInformations);
    }
}