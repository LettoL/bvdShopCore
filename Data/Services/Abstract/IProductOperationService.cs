using Data.ViewModels;

namespace Data.Services.Abstract
{
    public interface IProductOperationService
    {
        void Supply(SupplyProductVM obj);
        void WriteOff(int productId, int supplyId, int amount);

        decimal RealizationProduct(ShopContext db, int productId, int amount, int saleId);

        void ChangePrice(int productId, decimal price);      
    }
}