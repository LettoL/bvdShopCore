using System.Collections.Generic;
using Data.Entities;
using Data.Enums;
using System.Linq;

namespace Data.Services.Abstract
{
    public interface ISaleInfoService
    {
        bool HasAdditionalProduct(int id);
        string BuyerTitle(int id);
        string FirstProductTitle(int id);
        PaymentType PaymentType(int id, IEnumerable<InfoMoney> infoMonies);

        IQueryable<SaleProduct> GetProductsBySaleId(ShopContext db, int saleId);
    }
}