using System;
using System.Linq;
using Base.Services.Abstract;
using Base.Services.Concrete;
using Data.Entities;
using Data.Enums;
using Data.FiltrationModels;
using Data.Services.Abstract;
using Data.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Data.Services.Concrete
{
    public class ProductService : BaseObjectService<Product>, IProductService
    {                                           
        private readonly ShopContext _context;
        private readonly IInfoMoneyService _infoMoneyService;
        private readonly IBaseObjectService<Booking> _bookingService;
        private readonly IBaseObjectService<SupplyProduct> _supplyProductService;
        private readonly IBaseObjectService<Supplier> _supplierService;
        private readonly IBaseObjectService<ProductInformation> _productInformationService;
        private readonly IBaseObjectService<BookingProduct> _bookingProductService;
                                                                    
        public ProductService(ShopContext context,
            IInfoMoneyService infoMoneyService,
            IBaseObjectService<SupplyProduct> supplyProductService,
            IBaseObjectService<Supplier> supplierService,
            IBaseObjectService<BookingProduct> bookingProductsService,
            IBaseObjectService<ProductInformation> productInformationService,
            IBaseObjectService<Booking> bookingService) : base(context)
        {
            _context = context;
            _infoMoneyService = infoMoneyService;
            _supplyProductService = supplyProductService;
            _supplierService = supplierService;
            _productInformationService = productInformationService;
            _bookingProductService = bookingProductsService;
            _bookingService = bookingService;
        }

        public Booking Booking(BookingVM booking, int userId)
        {
            var shopId = _context.Users.First(x => x.Id == userId).ShopId;

            var productsIds = booking.Products.Select(x => x.Id);
            var products = _context.Products.Where(x => productsIds.Contains(x.Id));

            Booking newBooking = new Booking()
            {
                Date = DateTime.Now.AddHours(3),
                ShopId = shopId,
                UserId = userId,
                CashSum = booking.CashSum,
                CashlessSum = booking.CashlessSum,
                Sum = booking.Sum,
                Discount = booking.Discount,
                Debt = booking.Sum - booking.CashSum - booking.CashlessSum,
                Pay = booking.CashSum + booking.CashlessSum,
                Status = BookingStatus.Open,
                PartnerId = booking.Buyer == "Обычный покупатель"
                    ? null
                    : _context.Partners.First(p => p.Title == booking.Buyer)?.Id,
                forRussian = booking.forRussian
            };

            var createdBooking = _bookingService.Create(newBooking);

            foreach(var product in booking.Products)
            {
                _bookingProductService.Create(new BookingProduct()
                {
                    Amount = product.Amount,
                    BookingId = createdBooking.Id,
                    ProductId = product.Id,
                    Additional = product.Additional,
                    Cost = product.Cost
                });
            }

            if(booking.CashSum > 0)
            {
                _infoMoneyService.Create(new InfoMoney()
                {
                    BookingId = newBooking.Id,
                    PaymentType = PaymentType.Cash,
                    MoneyWorkerId = shopId,
                    Sum = booking.CashSum,
                    MoneyOperationType = MoneyOperationType.Booking
                });
            }

            if(booking.CashlessSum > 0)
            {
                _infoMoneyService.Create(new InfoMoney()
                {
                    BookingId = newBooking.Id,
                    PaymentType = PaymentType.Cashless,
                    MoneyWorkerId = booking.MoneyWorkerId,
                    Sum = booking.CashlessSum,
                    MoneyOperationType = MoneyOperationType.Booking
                });
            }
          
            return createdBooking;
        }

        public int BookedProducts(ShopContext db, int id, int shopId)
        {
            return db.BookingProducts
                .Where(x => x.ProductId == id 
                            && x.Product.ShopId == shopId 
                            && x.Booking.Status == BookingStatus.Open)
                .Sum(x => x.Amount);
        }

        public void Supply(SupplyProductVM obj)
        {
            var product = _context.Products.FirstOrDefault(x => x.Title == obj.Name && x.ShopId == obj.ShopId);

            if (product == null)
            {
                var productForCopy = _context.Products.FirstOrDefault(x => x.Id == obj.ProductId);

                product = _context.Products.Add(new Product()
                {
                    Title = productForCopy.Title,
                    Code = productForCopy.Code,
                    Reserv = productForCopy.Reserv,
                    BookingAmount = 0,
                    Cost = productForCopy.Cost,
                    CategoryId = productForCopy.CategoryId,
                    ShopId = obj.ShopId,
                }).Entity;

            }

            var supplyHistory = _context.SupplyHistories.Add(new SupplyHistory()).Entity;

            InfoProduct info;

            if (obj.SupplierId != 0)
            {
                info = new InfoProduct()
                {
                    Amount = obj.Amount,
                    Date = DateTime.Now.AddHours(3),
                    ProductId = product.Id,
                    ShopId = product.ShopId,
                    SupplierId = obj.SupplierId,
                    Type = InfoProductType.Supply,
                    SupplyHistoryId = supplyHistory.Id
                };

                var supplyProduct = new SupplyProduct()
                {
                    ProductId = product.Id,
                    SupplierId = obj.SupplierId,
                    TotalAmount = obj.Amount,
                    RealizationAmount = obj.Realization == SupplyType.ForRealization ? obj.Amount : 0,
                    AdditionalCost = obj.AdditionalCost / obj.Amount,
                    ProcurementCost = obj.ProcurementCost / obj.Amount,
                    FinalCost = (obj.ProcurementCost / obj.Amount) + (obj.AdditionalCost / obj.Amount),
                    StockAmount = obj.Amount,
                    SupplyHistoryId = supplyHistory.Id
                };
           
                _context.SupplyProducts.Add(supplyProduct);

                if (obj.Realization == SupplyType.DeferredPayment)
                {
                    _context.DeferredSupplyProducts.Add(new DeferredSupplyProduct()
                    {
                        Date = obj.Date,
                        SupplyProductId = supplyProduct.Id
                    });
                }
                
            }
            else
            {
                info = new InfoProduct()
                {
                    Amount = obj.Amount,
                    Date = DateTime.Now.AddHours(3),
                    ProductId = product.Id,
                    ShopId = product.ShopId,
                    Type = InfoProductType.Supply,
                    SupplyHistoryId = supplyHistory.Id
                };

                _context.SupplyProducts.Add(new SupplyProduct()
                {
                    ProductId = product.Id,
                    TotalAmount = obj.Amount,
                    RealizationAmount = obj.Realization == SupplyType.ForRealization ? obj.Amount : 0,
                    AdditionalCost = obj.AdditionalCost / obj.Amount,
                    ProcurementCost = obj.ProcurementCost / obj.Amount,
                    FinalCost = (obj.ProcurementCost / obj.Amount) + (obj.AdditionalCost / obj.Amount),
                    StockAmount = obj.Amount,
                    SupplyHistoryId = supplyHistory.Id
                });
            }

            _context.InfoProducts.Add(info);
            _context.SaveChanges();
        }

        public void SupplyAnnulment(int id)
        {
            var CurrentInfoProduct = _context.InfoProducts.FirstOrDefault(x => x.Id == id);
            
            var supplyHistory = _context.SupplyHistories
                .Include(x => x.InfoProducts)
                .Include(x => x.SupplyProducts)
                .FirstOrDefault(x => x.Id == CurrentInfoProduct.SupplyHistoryId);

            if (supplyHistory != null)
            {
                foreach (var supplyProduct in supplyHistory.SupplyProducts)
                {
                    _context.SupplyProducts.Remove(supplyProduct);
                }

                foreach (var infoProduct in supplyHistory.InfoProducts)
                {
                    _context.InfoProducts.Remove(infoProduct);
                }

                _context.SupplyHistories.Remove(supplyHistory);
            }
            else
            {
                _context.InfoProducts.Remove(CurrentInfoProduct);
            }
            

            
            _context.SaveChanges();
        }

        public IQueryable<Product> Filtration(ShopContext db, ProductFiltrationModel model)
        {
            var supplyProducts = db.SupplyProducts.ToList();
            
            var query = db.Products
                .Where(x => model.categoryId == 0 || x.CategoryId == model.categoryId)
                .Where(x => model.shopId == 0 || x.ShopId == model.shopId)
                .ToList()
                .Where(x => model.all == "true" || supplyProducts
                                .Where(s => s.ProductId == x.Id)
                                .Sum(s => s.StockAmount) > 0)
                .Where(x => model.title == null || x.Title.Contains(model.title))
                .AsQueryable();

            return query;
        }
    }
}