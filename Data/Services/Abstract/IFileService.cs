namespace Data.Services.Abstract
{
    public interface IFileService
    {
        void ExportProducts(ShopContext db, int shopId, string fileName);
    }
}