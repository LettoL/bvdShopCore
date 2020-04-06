using System;
using System.Linq;
using Base.Services.Abstract;
using Data.Entities;
using Data.Enums;
using Data.FiltrationModels;
using Data.ViewModels;

namespace Data.Services.Abstract
{
    public interface ISaleService : IBaseObjectService<Sale>
    {
        Sale Create(ShopContext db, SaleCreateVM obj, int userId);

        Sale CreatePostPayment(ShopContext db, SaleCreateVM obj, int userId);

        IQueryable<Sale> DeferredSalesFromStock(ShopContext context);

        IQueryable<Sale> SalesWithOpenPayments(ShopContext context);

        IQueryable<Sale> GetSalesByShop(int id);

        IQueryable<Sale> GetSalesByPartners();

        IQueryable<Sale> GetSalesByRussian();

        string ProductTitle(int id);

        void ClosePostPayment(int id, int moneyWorkerType, int moneyWorkerId, int moneyWorkerCashlessId, decimal totalCost);

        void ChangeProductProcurementCost(int productId, int saleId, decimal procurementCost);

        bool ForTest(int id);

        IQueryable<Sale> Filtration(SaleFiltrationModel model);
    }
}
