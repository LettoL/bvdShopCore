namespace Data.Services.Abstract
{
    public interface IFileService
    {
        void ExportProducts(int shopId, string fileName);
    }
}