using Data.ViewModels;
using System.Linq;

namespace Data.Services.Abstract
{
    public interface ISaleStatisticService
    {
        IQueryable<SaleByCategoryVM> GetSalesByCategoriesStats();
        IQueryable<SaleListVM> SaleList();
    }
}
