using Base.Services.Abstract;
using Data.Entities;
using Data.Enums;
using Data.Services.Abstract;
using System.Linq;

namespace Data.Services.Concrete
{
    public class BookingProductInformationService : IBookingProductInformationService
    {
        private readonly IBaseObjectService<BookingProduct> _bookingProductService;

        public BookingProductInformationService(IBaseObjectService<BookingProduct> bookingProductService)
        {
            _bookingProductService = bookingProductService;
        }

        public IQueryable<BookingProduct> GetBookedProductsInShop(int shopId)
        {
            return _bookingProductService.All()
                .Where(x => x.Booking.ShopId == shopId && x.Booking.Status == BookingStatus.Open);
        }

        public IQueryable<BookingProduct> GetBookingProductByBooking(ShopContext db, int bookingId)
        {
            return db.BookingProducts.Where(x => x.BookingId == bookingId);
        }

        public int GetAmountOfBookedProductsInShop(int shopId)
        {
            return GetBookedProductsInShop(shopId)
                .Sum(x => x.Amount);
        }
    }
}
