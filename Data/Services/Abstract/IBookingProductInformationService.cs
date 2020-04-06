using Data.Entities;
using System.Linq;

namespace Data.Services.Abstract
{
    public interface IBookingProductInformationService
    {
        IQueryable<BookingProduct> GetBookedProductsInShop(int shopId);
        int GetAmountOfBookedProductsInShop(int shopId);

        IQueryable<BookingProduct> GetBookingProductByBooking(ShopContext db, int bookingId);
    }
}
