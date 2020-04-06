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
        PaymentType PaymentType(int id);

        IQueryable<SaleProduct> GetProductsBySaleId(ShopContext db, int saleId);
    }
}