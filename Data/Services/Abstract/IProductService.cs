using Base.Services.Abstract;
using Data.Entities;
using Data.ViewModels;
using System.Linq;
using Data.FiltrationModels;

namespace Data.Services.Abstract
{
    public interface IProductService : IBaseObjectService<Product>
    {
        Booking Booking(BookingVM booking, int userId);

        //int InStock(int id);

        //void Supply(SupplyProductVM obj);

        void SupplyAnnulment(int id);

        int BookedProducts(ShopContext db, int id, int shopId);

        IQueryable<Product> Filtration(ProductFiltrationModel model);
    }
}
