using System.Linq;
using Base.Services.Abstract;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Data.Services.Abstract;
using Data.ViewModels;
using Data.Enums;
using WebUI.ViewModels;
using System;
using System.Threading.Tasks;
using Data;
using Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace WebUI.Controllers
{
    [Authorize]
    public class ManagerController : Controller
    {
        private readonly IBaseObjectService<Partner> _partnerService;
        private readonly IBaseObjectService<User> _userService;
        private readonly IBaseObjectService<Category> _categoryService;
        private readonly ISaleService _saleService;
        private readonly IInfoMoneyService _infoMoneyService;
        private readonly IProductService _productService;
        private readonly IBaseObjectService<Booking> _bookingService;
        private readonly IShopService _shopService;
        private readonly IBaseObjectService<BookingProduct> _bookingProductService;
        private readonly IBaseObjectService<SupplyProduct> _supplyProduct;
        private readonly IBaseObjectService<SaleProduct> _saleProductService;
        private readonly IBaseObjectService<Supplier> _supplierService;
        private readonly IBaseObjectService<SaleInformation> _saleInformationService;
        private readonly IBaseObjectService<ExpenseCategory> _expenseCategoryService;
        private readonly IBaseObjectService<Expense> _expenseService;
        private readonly IBaseObjectService<ProductInformation> _productInformation;
        private readonly IProductOperationService _productOperationService;
        private readonly IMoneyOperationService _moneyOperationService;
        private readonly ShopContext _db;
        private readonly IMoneyStatisticService _moneyStatisticService;
        private readonly IBookingProductInformationService _bookingProductInformationService;
        private readonly ISaleInfoService _saleInfoService;

        public ManagerController(IBaseObjectService<Partner> partnerService,
            IBaseObjectService<User> userService,
            ISaleService saleService,
            IInfoMoneyService infoMoneyService,
            IProductService productService,
            IBaseObjectService<Booking> bookingService,
            IShopService shopService,
            IBaseObjectService<BookingProduct> bookingProductService,
            IBaseObjectService<Category> categoryService,
            IBaseObjectService<SupplyProduct> supplyProduct,
            IBaseObjectService<SaleProduct> saleProductService,
            IBaseObjectService<Supplier> supplierSerivce,
            IBaseObjectService<SaleInformation> saleInformationService,
            IBaseObjectService<ExpenseCategory> expenseCategoryService,
            IBaseObjectService<Expense> expenseService,
            IBaseObjectService<ProductInformation> productInformation,
            ShopContext db,
            IProductOperationService productOperationService,
            IMoneyOperationService moneyOperationService,
            IMoneyStatisticService moneyStatisticService,
            IBookingProductInformationService bookingProductInformationService,
            ISaleInfoService saleInfoService)
        {
            _partnerService = partnerService;
            _userService = userService;
            _saleService = saleService;
            _infoMoneyService = infoMoneyService;
            _productService = productService;
            _bookingService = bookingService;
            _shopService = shopService;
            _bookingProductService = bookingProductService;
            _categoryService = categoryService;
            _supplyProduct = supplyProduct;
            _saleProductService = saleProductService;
            _supplierService = supplierSerivce;
            _saleInformationService = saleInformationService;
            _expenseCategoryService = expenseCategoryService;
            _expenseService = expenseService;
            _productInformation = productInformation;
            _productOperationService = productOperationService;
            _db = db;
            _moneyOperationService = moneyOperationService;
            _moneyStatisticService = moneyStatisticService;
            _bookingProductInformationService = bookingProductInformationService;
            _saleInfoService = saleInfoService;
        }

        public IActionResult Index()
        {
            var userName = HttpContext.User.Identity.Name;
            var user = _db.Users.FirstOrDefault(u => u.Login == userName);
            var shop = _db.Shops.FirstOrDefault(x => x.Id == user.ShopId);

            var salesProductsToday = _db.SalesProducts
                .Where(x => x.Sale.Date.DayOfYear == DateTime.Now/*.AddHours(3)*/.DayOfYear
                            && x.Sale.Date.Year == DateTime.Now/*.AddHours(3)*/.Date.Year 
                            && x.Sale.ShopId == user.ShopId)
                .Select(x => new SaleProduct()
                {
                    Product = x.Product,
                    Amount = x.Amount
                }).ToList();

            ViewBag.SalesProducts = salesProductsToday;

            ViewBag.SalesAmount = salesProductsToday.Sum(sp => sp.Amount);

            ViewBag.Expenses = _moneyStatisticService.ShopExpenses(_db, shop.Id);

            ViewBag.Sum = _shopService.CashOnHand(_db, shop.Id);

            var infoMoneys = _db.InfoMonies.ToList();
            var saleProducts = _db.SalesProducts
                .Include(x => x.Product)
                .ToList();

            ViewBag.DefferedSales = _db.Sales
                .Where(s => s.Payment == false && s.ShopId == user.ShopId
                                               && _db.SaleInformations.Count(x =>
                                                   x.SaleId == s.Id && x.SaleType == SaleType.DefferedSaleFromStock) > 0)
                .ToList()
                .Where(s => s.Sum - infoMoneys.Where(z => z.SaleId == s.Id).Sum(x => x.Sum) > 0)
                .Select(s => new SaleVM()
                {
                    Id = s.Id,
                    Date = s.Date.ToString("dd.MM.yyyy"),
                    Sum = infoMoneys.Where(x => x.SaleId == s.Id).Sum(x => x.Sum),
                    ShopTitle = s.Shop.Title,
                    ProductTitle = saleProducts
                        .FirstOrDefault(x => x.SaleId == s.Id).Product.Title,
                    Rest = s.Sum - infoMoneys.Where(x => x.SaleId == s.Id).Sum(x => x.Sum),
                    Total = s.Sum
                }).ToList();

            ViewBag.OpenSales = _db.Sales
                .Where(x => x.Payment == false
                            && x.ShopId == user.ShopId
                            && _db.SaleInformations.Count(z =>
                                z.SaleId == x.Id
                                && z.SaleType == SaleType.DefferedSale) > 0)
                .OrderByDescending(x => x.Id)
                .Select(s => new SaleVM()
                {
                    Id = s.Id,
                    Date = s.Date.ToString("dd.MM.yyyy"),
                    Sum = s.Sum,
                    ProductTitle = _db.SalesProducts
                        .FirstOrDefault(x => x.SaleId == s.Id).Product.Title
                });

            ViewBag.SalePayments = _db.InfoMonies
                .Where(x => (x.SaleId != null || x.BookingId != null)
                            && x.Date.Date == DateTime.Now/*.AddHours(3)*/.Date.Date
                            && (x.Sale.ShopId == shop.Id || x.Booking.ShopId == shop.Id ))
                .OrderByDescending(x => x.Id)
                .Select(x => new SalePaymentVM
                {
                    Date = x.Date.ToShortTimeString(),
                    SaleNumber = x.SaleId == null 
                        ? x.BookingId.ToString()
                        : x.SaleId.ToString(),
                    MoneyWorker = x.MoneyWorker.Title,
                    PaymentType = x.PaymentType == PaymentType.Cash
                        ? "Наличный"
                        : "Безналичный",
                    Sum = x.Sum.ToString(),
                    SaleId = x.SaleId,
                    BookingId = x.BookingId,
                    Discount = x.SaleId == null 
                        ? x.Booking.Discount
                        : x.Sale.Discount,
                    ForRF = x.SaleId == null
                        ? x.Booking.forRussian
                        : x.Sale.ForRussian,

                    OperationType = (x.SaleId == null)
                        ? "Бронирование"
                        : x.Sale.SaleType == SaleType.Sale
                            ? "Продажа"
                            : x.Sale.SaleType == SaleType.SaleFromStock
                                ? "Продажа со склада поставщика"
                                : x.Sale.SaleType == SaleType.DefferedSaleFromStock
                                    ? "Отложенная продажа со склада поставщика"
                                    : x.Sale.SaleType == SaleType.DefferedSale
                                        ? "Продажа с отложенным платежом"
                                        : x.Sale.SaleType == SaleType.Booking
                                            ? "Закрытое бронирование"
                                            : "Неопределено"
                })
                .ToList()
                .Select(x => new SalePaymentVM
                {
                    Date = x.Date,
                    SaleNumber = x.SaleNumber,
                    MoneyWorker = x.MoneyWorker,
                    PaymentType = x.PaymentType,
                    Sum = x.Sum,
                    OperationType = x.OperationType,
                    Discount = x.Discount,
                    ForRF = x.ForRF,
                    SaleProducts = x.SaleId != null                    
                        ? saleProducts.Where(z => z.SaleId == x.SaleId.Value)//_saleInfoService.GetProductsBySaleId(x.SaleId.Value)
                            .Select(z => new SaleProductItemVM
                            {
                                Title = z.Product.Title,
                                Amount = z.Amount.ToString()
                            }).ToList()
                        : _bookingProductInformationService.GetBookingProductByBooking(_db, x.BookingId.Value)
                            .Select(z => new SaleProductItemVM
                            {
                                Title = z.Product.Title,
                                Amount = z.Amount.ToString()
                            }).ToList(),                    
                        
                }).Concat(
                _db.Sales
                    .Where(x => x.SaleType == SaleType.SaleFromStock
                                              && x.Date.DayOfYear == DateTime.Now/*.AddHours(3)*/.DayOfYear
                                              && x.Date.Year == DateTime.Now/*.AddHours(3)*/.Year
                                              && x.ShopId == shop.Id
                                              && _db.SaleInformations.Where(z => z.SaleId == x.Id).Count() > 0)
                    .Select(x => new SalePaymentVM
                    {
                        Date = x.Date.ToShortTimeString(),
                        SaleNumber = x.Id.ToString(),
                        MoneyWorker = x.Shop.Title,
                        PaymentType = x.CashSum > 0 && x.CashlessSum > 0
                            ? "Смешанный"
                            : x.CashSum > 0
                                ? "Наличный"
                                : "Безналичный",
                        Sum = x.CashSum > 0 && x.CashlessSum > 0
                            ? x.CashSum.ToString() + " и " + x.CashlessSum.ToString()
                            : x.CashSum > 0
                                ? x.CashSum.ToString()
                                : x.CashlessSum.ToString(),
                        OperationType = "Продажа со склада поставщика (не закрыта)",
                        Discount = x.Discount,
                        ForRF = x.ForRussian,
                        SaleId = x.Id
                    }).ToList()
                    .Select(x => new SalePaymentVM {
                        Date = x.Date,
                        SaleNumber = x.SaleNumber,
                        MoneyWorker = x.PaymentType == "Смешанный" 
                            ? x.MoneyWorker + " и " + _db.SaleInformations.Where(z => z.SaleId == x.SaleId).Select(z => z.MoneyWorkerForCashlessIncome.Title).FirstOrDefault()
                            : x.PaymentType == "Безналичный"
                                ? _db.SaleInformations.Where(z => z.SaleId == x.SaleId).Select(z => z.MoneyWorkerForCashlessIncome.Title).FirstOrDefault()
                                : x.MoneyWorker,
                        PaymentType = x.PaymentType,
                        Sum = x.Sum,
                        OperationType = x.OperationType,
                        Discount = x.Discount,
                        ForRF = x.ForRF,
                        SaleProducts = _saleInfoService.GetProductsBySaleId(_db, x.SaleId.Value)
                            .Select(z => new SaleProductItemVM
                            {
                                Title = z.Product.Title,
                                Amount = z.Amount.ToString()
                            }).ToList()
                    })).OrderByDescending(x => x.Date);

            return View();
        }

        [HttpGet]
        public IActionResult Realization()
        {
            ViewBag.Partners = _partnerService.All();

            var userName = HttpContext.User.Identity.Name;
            ViewBag.userId = _userService.All().First(u => u.Login == userName).Id;

            return View();
        }

        [HttpPost]
        public IActionResult RealizationPost([FromBody] RealizationVM json)
        {
            SaleCreateVM saleCreate = new SaleCreateVM()
            {
                UserId = json.UserId,
                MoneyWorkerId = json.MoneyWorkerId,
                Discount = json.Discount,
                AdditionalComment = json.AdditionalComment,
                Comment = json.Comment,
                PaymentType = json.Cashless 
                    ? PaymentType.Cashless 
                    : PaymentType.Cash,
                CashSum = json.CashSum,
                CashlessSum = json.CashlessSum,
                Sum = json.Sum,
                Products = json.Products,
                PartnerId = json.Buyer == "Обычный покупатель"
                    ? null
                    : _db.Partners.First(p => p.Title == json.Buyer)?.Id,
                Payment = json.Payment,
                ForRussian = json.ForRussian
            };

            var createdSale = _saleService.Create(_db, saleCreate, json.UserId);
            
            /*var client = new MongoClient("mongodb+srv://admin:1234@cluster0-qpif1.azure.mongodb.net/test?retryWrites=true&w=majority");
            var db = client.GetDatabase("bvdShop");

            var managers = db.GetCollection<Manager>("managers")
                .Find(manager => true)
                .ToList();

            var managerId = managers.FirstOrDefault(x => x.Name == json.Manager)?.Id;
            
            db.GetCollection<SaleManager>("saleManagers")
                .InsertOne(new SaleManager()
                {
                    ManagerId = managerId,
                    SaleId = createdSale.Id
                });*/

            return RedirectToAction("CheckPrint", new { saleId = createdSale.Id, operationSum = json.CashSum + json.CashlessSum });
        }

        [HttpGet]
        public IActionResult CheckPrint(int? saleId, int? bookingId, decimal operationSum)
        {
            if(saleId != null)
            {
                var sale = _saleService.All().FirstOrDefault(x => x.Id == saleId);

                ViewBag.SaleProducts = _saleProductService.All()
                    .Where(x => x.SaleId == sale.Id)
                    .Include(x => x.Product);

                ViewBag.Sale = true;
                ViewBag.OperationSum = operationSum;

                if (sale.SaleType != SaleType.SaleFromStock)
                    ViewBag.TotalOperationSum = _infoMoneyService.All()
                        .Where(x => x.SaleId == sale.Id)
                        .Sum(x => x.Sum);
                else
                    ViewBag.TotalOperationSum = operationSum;

                return View(sale);
            }
            else
            {
                var booking = _bookingService.All().FirstOrDefault(x => x.Id == bookingId);

                ViewBag.SaleProducts = _bookingProductService.All()
                    .Where(x => x.BookingId == bookingId)
                    .Include(x => x.Product);

                ViewBag.OperationSum = operationSum;
                ViewBag.TotalOperationSum = _infoMoneyService.All()
                    .Where(x => x.BookingId == booking.Id)
                    .Sum(x => x.Sum);

                ViewBag.Sale = false;

                return View(booking);
            }
        }

        [HttpGet]
        public IActionResult Booking()
        {
            var userName = HttpContext.User.Identity.Name;
            var userId = _userService.All().First(u => u.Login == userName).Id;

            ViewBag.UserId = userId;
            ViewBag.Partners = _partnerService.All();

            return View();
        }

        [HttpPost]
        public IActionResult Booking([FromBody] BookingVM booking)
        {
            var createdBooking = _productService.Booking(booking, booking.UserId);

            return RedirectToAction("CheckPrint", new { bookingId = createdBooking.Id, operationSum = booking.CashSum + booking.CashlessSum });
        }

        [HttpGet]
        public IActionResult BookingList()
        {
            var userName = HttpContext.User.Identity.Name;
            User user = _userService.All().First(u => u.Login == userName);
            Shop shop = _shopService.All().First(s => s.Id == user.ShopId);


            return View(_bookingService.All()
                .Where(x => x.ShopId == shop.Id)
                .OrderByDescending(x => x.Id)
                .Select(x => new BookingListVM()
                {
                    Date = x.Date,
                    Id = x.Id,
                    Debt = x.Debt,
                    Pay = x.Pay,
                    Sum = x.Sum,
                    Status = x.Status,
                    ProductTitle = _bookingProductService.All()
                        .Include(z => z.Product).FirstOrDefault(z => z.BookingId == x.Id).Product.Title ?? ""
                }));

        }

        [HttpGet]
        public IActionResult BookingDelete(int id)
        {
            var booking = _db.Bookings.FirstOrDefault(x => x.Id == id);

            var bookingProducts = _db.BookingProducts.Where(x => x.BookingId == booking.Id).ToList();

            foreach (var bookingProduct in bookingProducts)
            {
                _db.BookingProducts.Remove(bookingProduct);
            }

            var infoMoneys = _db.InfoMonies.Where(x => x.BookingId == booking.Id).ToList();

            foreach (var infoMoney in infoMoneys)
            {
                _db.InfoMonies.Remove(infoMoney);
            }

            _db.Bookings.Remove(booking);

            _db.SaveChanges();

            return RedirectToAction("BookingList");
        }

        [HttpGet]
        public IActionResult BookingDetail(int id)
        {
            ViewBag.BookingProducts = _bookingProductService.All()
                .Where(bp => bp.BookingId == id)
                .Select(bp => new BookingDetailVM()
                {
                    ProductTitle = bp.Product.Title,
                    ProductCode = bp.Product.Code,
                    Amount = bp.Amount,
                    Additional = bp.Additional
                });

            return View(_bookingService.All().FirstOrDefault(b => b.Id == id));
        }

        [HttpGet]
        public IActionResult BookingClose()
        {
            return View();
        }

        [HttpPost]
        public IActionResult BookingCloseAdditional(int id)
        {
            return PartialView(_bookingService
                .All()
                .FirstOrDefault(b => b.Id == id));
        }

        //TODO: В сервис это говно
        [HttpPost]
        public IActionResult BookingClose(int id, decimal cashSum, decimal cashlessSum, int moneyWorkerId, bool cashless)
        {
            Booking booking = _bookingService.All().First(b => b.Id == id);
            booking.Debt -= (cashSum + cashlessSum);
            booking.Pay += (cashSum + cashlessSum);

            if (booking.Debt <= 0)
                booking.Status = BookingStatus.Close;

            _bookingService.Update(booking);

            var userName = HttpContext.User.Identity.Name;
            User user = _userService.All().First(u => u.Login == userName);

            if (cashSum > 0)
            {
                _infoMoneyService.Create(new InfoMoney()
                {
                    PaymentType = PaymentType.Cash,
                    MoneyWorkerId = user.ShopId,
                    MoneyOperationType = MoneyOperationType.Booking,
                    Sum = cashSum,
                    BookingId = booking.Id
                });
            }

            if (cashlessSum > 0)
            {
                _infoMoneyService.Create(new InfoMoney()
                {
                    PaymentType = PaymentType.Cashless,
                    MoneyWorkerId = moneyWorkerId,
                    MoneyOperationType = MoneyOperationType.Booking,
                    Sum = cashlessSum,
                    BookingId = booking.Id
                });
            }

            int createdSaleId = 0;

            if(booking.Status == BookingStatus.Close)
            {
                var b_products = _bookingProductService.All()
                    .Where(bp => bp.BookingId == booking.Id)
                    .ToList();

                decimal primeCost = 0;

                var sale = new Sale()
                {
                    Date = DateTime.Now.AddHours(3),
                    ShopId = user.ShopId.Value,
                    PartnerId = booking.PartnerId,
                    Sum = booking.Sum,
                    CashSum = _infoMoneyService.All()
                        .Where(x => x.BookingId == booking.Id && x.PaymentType == PaymentType.Cash)
                        .Sum(x => x.Sum),
                    CashlessSum = _infoMoneyService.All()
                        .Where(x => x.BookingId == booking.Id && x.PaymentType == PaymentType.Cashless)
                        .Sum(x => x.Sum),
                    Payment = true,
                    SaleType = SaleType.Booking,
                    Discount = booking.Discount,
                    ForRussian = booking.forRussian
                };

                var createdSale = _saleService.Create(sale);

                _infoMoneyService.All()
                    .Where(x => x.BookingId == booking.Id)
                    .ToList()
                    .ForEach(x => {
                        x.SaleId = sale.Id;
                        _infoMoneyService.Update(x);
                    });

                _saleInformationService.Create(new SaleInformation
                {
                    SaleId = createdSale.Id,
                    SaleType = SaleType.Booking
                });

                foreach (var p in b_products)
                {
                    _saleProductService.Create(new SaleProduct()
                    {
                        Amount = p.Amount,
                        ProductId = p.ProductId,
                        SaleId = sale.Id,
                        Additional = p.Additional,
                        Cost = p.Cost
                    });

                    primeCost += _productOperationService.RealizationProduct(_db, p.ProductId, p.Amount, createdSale.Id);
                }

                sale.PrimeCost = primeCost;
                sale.Margin = sale.Sum - sale.PrimeCost;
                createdSaleId = createdSale.Id;

                _saleService.Update(createdSale);
            }
            
            return RedirectToAction("CheckPrint", new { saleId = createdSaleId, operationSum = cashSum + cashlessSum });
        }

        

        [HttpGet]
        public IActionResult Products()
        {
            var userName = HttpContext.User.Identity.Name;
            User user = _userService.All().First(u => u.Login == userName);
            Shop shop = _shopService.All().First(s => s.Id == user.ShopId);

            ViewBag.Categories = _categoryService.All();
            ViewBag.Shops = _shopService.All();
            ViewBag.UserId = user.Id;
            
            /*var bookedProducts = _db.BookingProducts
                .Where(x => x.Booking.Status == BookingStatus.Open)
                .Select(x => new
                {
                    ProductId = x.ProductId,
                    Amount = x.Amount
                })
                .ToList()
                .GroupBy(x => x.ProductId)
                .Select(x => new
                {
                    ProductId = x.Key,
                    Amount = x.Sum(z => z.Amount)
                })
                .ToList();

            var productsInStock = _db.SupplyProducts
                .Where(x => x.StockAmount > 0)
                .Select(x => new
                {
                    ProductId = x.ProductId,
                    Title = x.Product.Title,
                    Cost = x.Product.Cost,
                    Shop = x.Product.Shop,
                    Category = x.Product.Category,
                    Code = x.Product.Code,
                    StockAmount = x.StockAmount
                })
                .ToList()
                .GroupBy(x => x.ProductId)
                .Select(x => new ProductVM
                {
                    Id = x.Key,
                    Amount = x.Sum(z => z.StockAmount)
                             - (bookedProducts.FirstOrDefault(z => z.ProductId == x.Key)?.Amount ?? 0),
                    Title = x.FirstOrDefault().Title,
                    Cost = x.FirstOrDefault().Cost,
                    Shop = x.FirstOrDefault().Shop,
                    Category = x.FirstOrDefault().Category,
                    Code = x.FirstOrDefault().Code,
                    BookedCount = bookedProducts.FirstOrDefault(z => z.ProductId == x.Key)?.Amount ?? 0
                })
                .ToList();*/

            var result = ProductService.GetProductsInStock(_db);

            return View(result);
        }

        [HttpGet]
        public IActionResult AllProducts()
        {
            var userName = HttpContext.User.Identity.Name;
            User user = _userService.All().First(u => u.Login == userName);
            Shop shop = _shopService.All().First(s => s.Id == user.ShopId);

            ViewBag.Categories = _categoryService.All();
            ViewBag.Shops = _shopService.All();
            ViewBag.UserId = user.Id;

            return View(ProductService.GetAllProducts(_db));
        }

        [HttpGet]
        public IActionResult ProductDetail(int id)
        {
            ProductVM product = _productService.All().Select(x => new ProductVM()
                {
                    Id = x.Id,
                    Category = x.Category,
                    Code = x.Code,
                    Cost = x.Cost,
                    Title = x.Title,
                    Shop = x.Shop
                }).ToList()
                .Select(x => new ProductVM()
                {
                    Id = x.Id,
                    Amount = _supplyProduct.All().Where(sp => sp.ProductId == x.Id)
                        .Sum(sp => sp.StockAmount),
                    Category = x.Category,
                    Code = x.Code,
                    Cost = x.Cost,
                    Title = x.Title,
                    Shop = x.Shop
                }).First(p => p.Id == id);


            //TODO: Сделать Join один поставщик - один SupplyProduct на отображение
            //Для таба с поставщиками
            ViewBag.SupplierProducts = _supplyProduct.All()
                .Where(x => x.ProductId == id && x.SupplierId != null)
                .Select(x => new SupplyProduct
                {
                    Id = x.Id,
                    Supplier = x.Supplier,
                    Product = x.Product,
                    TotalAmount = x.TotalAmount,
                    RealizationAmount = x.RealizationAmount
                });

            //Для таба со стоимостью товара
            ViewBag.SupplyProduct = _supplyProduct.All()
                .Where(x => x.ProductId == id)
                .Select(x => new SupplyProduct
                {
                    Id = x.Id,
                    Product = x.Product,
                    TotalAmount = x.TotalAmount,
                    StockAmount = x.StockAmount,
                    AdditionalCost = x.AdditionalCost,
                    ProcurementCost = x.ProcurementCost,
                    FinalCost = x.FinalCost
                });

            return View(product);
        }

        public IActionResult Expense()
        {
            var userName = HttpContext.User.Identity.Name;
            User user = _userService.All().First(u => u.Login == userName);

            ViewBag.ShopId = _shopService.All().FirstOrDefault(s => s.Id == user.ShopId);
            ViewBag.CategoryExpense = _expenseCategoryService.All();

            return View();
        }

        [HttpPost]
        public IActionResult Expense(ExpenseVM expense)
        {
            var userName = HttpContext.User.Identity.Name;
            var userId = _userService.All().First(u => u.Login == userName).Id;

            _moneyOperationService.Expense(expense.MoneyWorkerId, expense.Sum,
                _moneyOperationService.PaymentTypeByMoneyWorker(expense.MoneyWorkerId),
                expense.ExpenseCategory, expense.Comment, _shopService.ShopByUserId(_db, userId).Id);

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("/Manager/GetMoneyWorkers/{value}")]
        public async Task<IActionResult> GetMoneyWorkers(int value)
        {
            var userName = HttpContext.User.Identity.Name;
            User user = _userService.All().First(u => u.Login == userName);

            if (value == 1) //Получить держателей карт
            {
                //TODO: _db???
                var cardKeepers = await _db.CardKeepers.Where(ck => ck.ForManager).ToListAsync();
                return Ok(cardKeepers);
            }

            if (value == 3)
            {
                //TODO: _db???
                var shop = await _db.Shops.Where(s => s.Id == user.ShopId).ToArrayAsync();
                return Ok(shop);
            }
                

            return BadRequest();
        }

        [HttpGet]
        [Route("/Manager/GetMoneyWorkersForSale/{value}")]
        public async Task<IActionResult> GetMoneyWorkersForSale(int value)
        {
            var userName = HttpContext.User.Identity.Name;
            User user = _userService.All().First(u => u.Login == userName);

            if (value == 1) //Получить держателей карт
            {
                //TODO: _db???
                var cardKeepers = await _db.CardKeepers.ToListAsync();
                return Ok(cardKeepers);
            }

            if (value == 2) // получить рассчетные счета
            {
                //TODO: _db???
                var scores = await _db.CalculatedScores.ToListAsync();
                return Ok(scores);
            }

            if (value == 3) // получить кассу
            {
                //TODO: _db???
                var shops = await _db.Shops.Where(x => x.Id == user.ShopId).ToListAsync();
                return Ok(shops);
            }

            return BadRequest();
        }

        public IActionResult SaleFromStock()
        {
            ViewBag.Partners = _partnerService.All();

            var userName = HttpContext.User.Identity.Name;
            ViewBag.userId = _userService.All().First(u => u.Login == userName).Id;

            return View();
        }

        [HttpPost]
        public IActionResult SaleFromStock([FromBody]SaleFromStockVM json)
        {
            SaleCreateVM saleCreate = new SaleCreateVM()
            {
                UserId = json.UserId,
                Discount = json.Discount,
                AdditionalComment = json.AdditionalComment,
                Comment = json.Comment,
                Sum = json.Sum,
                CashSum = json.Cash,
                CashlessSum = json.Cashless,
                Products = json.Products,
                PartnerId = json.Buyer == "Обычный покупатель"
                    ? null
                    : _partnerService.All().First(p => p.Title == json.Buyer)?.Id,
                SaleType = SaleType.SaleFromStock,
                ForRussian = json.ForRussian
            };

            var createdSale = _saleService.CreatePostPayment(_db, saleCreate, json.UserId);
            var shop = _shopService.ShopByUserId(_db, json.UserId);

            _db.SaleInformations.Add(new SaleInformation()
            {
                Sale = createdSale,
                MoneyWorkerForIncomeId = shop.Id,
                MoneyWorkerForCashlessIncomeId = json.MoneyWorkerIdForCashlessIncome == 0
                    ? null
                    : json.MoneyWorkerIdForCashlessIncome,
                SaleType = SaleType.SaleFromStock
            });

            _db.SaveChanges();
            
            /*var client = new MongoClient("mongodb+srv://admin:1234@cluster0-qpif1.azure.mongodb.net/test?retryWrites=true&w=majority");
            var db = client.GetDatabase("bvdShop");

            var managers = db.GetCollection<Manager>("managers")
                .Find(manager => true)
                .ToList();

            var managerId = managers.FirstOrDefault(x => x.Name == json.Manager)?.Id;
            
            db.GetCollection<SaleManager>("saleManagers")
                .InsertOne(new SaleManager()
                {
                    ManagerId = managerId,
                    SaleId = createdSale.Id
                });*/

            return RedirectToAction("CheckPrint", new { saleId = createdSale.Id, operationSum = json.Cash + json.Cashless });
        }

        //TODO: В сервис
        [HttpGet]
        public IActionResult CloseOpenSale(int id)
        {
            var sale = _saleService.All().FirstOrDefault(x => x.Id == id);
            sale.Payment = true;

            var saleInformation = _saleInformationService.All().FirstOrDefault(x => x.SaleId == sale.Id);
            var infoMoneys = _infoMoneyService.All().Where(x => x.SaleId == sale.Id);
            

            if(infoMoneys.Count() == 0)
            {
                if (sale.CashSum > 0)
                    _infoMoneyService.Create(new InfoMoney()
                    {
                        MoneyWorkerId = sale.ShopId,
                        Sum = sale.CashSum,
                        SaleId = sale.Id,
                        PaymentType = PaymentType.Cash,
                        MoneyOperationType = MoneyOperationType.Sale
                    });

                if (sale.CashlessSum > 0)
                    _infoMoneyService.Create(new InfoMoney()
                    {
                        MoneyWorkerId = saleInformation.MoneyWorkerForIncomeId,
                        Sum = sale.CashlessSum,
                        SaleId = sale.Id,
                        PaymentType = PaymentType.Cashless,
                        MoneyOperationType = MoneyOperationType.Sale
                    });
            }
            
            _db.SaleInformations.Remove(saleInformation);

            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult DefferedSaleFromStock()
        {
            ViewBag.Partners = _partnerService.All();

            var userName = HttpContext.User.Identity.Name;
            var userId = _userService.All().First(u => u.Login == userName).Id;

            ViewBag.UserId = userId;

            return View();
        }

        [HttpPost]
        public IActionResult DefferedSaleFromStock([FromBody]DefferedSaleFromStockVM sale)
        {
            var user = _userService.All().FirstOrDefault(x => x.Id == sale.UserId);

            SaleCreateVM saleCreate = new SaleCreateVM()
            {
                UserId = sale.UserId,
                Discount = sale.Discount,
                CashSum = sale.CashSum,
                CashlessSum = sale.CashlessSum,
                Comment = sale.Comment,
                Sum = sale.Sum,
                Products = sale.Products,
                PartnerId = sale.Buyer == "Обычный покупатель"
                    ? null
                    : _partnerService.All().First(p => p.Title == sale.Buyer)?.Id,
                SaleType = SaleType.DefferedSaleFromStock,
                ForRussian = sale.ForRussian
            };

            var createdSale = _saleService.CreatePostPayment(_db, saleCreate, sale.UserId);

            _db.SaleInformations.Add(new SaleInformation()
            {
                Sale = createdSale,        
                SaleType = SaleType.DefferedSaleFromStock
            });

            if (sale.CashSum > 0)
                _infoMoneyService.Create(new InfoMoney()
                {
                    MoneyWorkerId = _shopService.All().FirstOrDefault(x => x.Id == user.ShopId).Id,
                    Sum = sale.CashSum,
                    Sale = createdSale,
                    PaymentType = PaymentType.Cash,
                    MoneyOperationType = MoneyOperationType.Sale
                });

            if (sale.CashlessSum > 0)
                _infoMoneyService.Create(new InfoMoney()
                {
                    MoneyWorkerId = sale.MoneyWorkerId,
                    Sum = sale.CashlessSum,
                    Sale = createdSale,
                    PaymentType = PaymentType.Cashless,
                    MoneyOperationType = MoneyOperationType.Sale
                });

            _db.SaveChanges();
            
            /*var client = new MongoClient("mongodb+srv://admin:1234@cluster0-qpif1.azure.mongodb.net/test?retryWrites=true&w=majority");
            var db = client.GetDatabase("bvdShop");

            var managers = db.GetCollection<Manager>("managers")
                .Find(manager => true)
                .ToList();

            var managerId = managers.FirstOrDefault(x => x.Name == sale.Manager)?.Id;
            
            db.GetCollection<SaleManager>("saleManagers")
                .InsertOne(new SaleManager()
                {
                    ManagerId = managerId,
                    SaleId = createdSale.Id
                });*/

            return RedirectToAction("CheckPrint", new { saleId = createdSale.Id, operationSum = sale.CashSum + sale.CashlessSum });
        }

        public IActionResult CloseDefferedSale(int id)
        {
            var userName = HttpContext.User.Identity.Name;
            ViewBag.userId = _userService.All().First(u => u.Login == userName).Id;

            ViewBag.Debt = _saleService.All().FirstOrDefault(x => x.Id == id).Sum -
                           _infoMoneyService.All().Where(x => x.SaleId == id).Sum(x => x.Sum);

            return View(_saleService
                .All()
                .FirstOrDefault(x => x.Id == id));
        }

        [HttpPost]
        public IActionResult CloseDefferedSale(int id, decimal cashSum, decimal cashlessSum, int moneyWorkerId, int userId)
        {
            Sale sale = _saleService.All().First(b => b.Id == id);
            User user = _userService.All().FirstOrDefault(x => x.Id == userId);

            if (cashSum > 0)
            {
                _infoMoneyService.Create(new InfoMoney()
                {
                    PaymentType = PaymentType.Cash,
                    MoneyWorkerId = user.ShopId,
                    MoneyOperationType = MoneyOperationType.Sale,
                    Sum = cashSum,
                    Sale = sale
                });
            }

            if (cashlessSum > 0)
            {
                _infoMoneyService.Create(new InfoMoney()
                {
                    PaymentType = PaymentType.Cashless,
                    MoneyWorkerId = moneyWorkerId,
                    MoneyOperationType = MoneyOperationType.Sale,
                    Sum = cashlessSum,
                    Sale = sale
                });
            }

            return RedirectToAction("CheckPrint", new { saleId = id, operationSum = cashSum + cashlessSum });
        }

        [HttpGet]
        public IActionResult SaleList()
        {
            var userName = HttpContext.User.Identity.Name;
            var user = _db.Users.First(u => u.Login == userName);

            ViewBag.UserId = user.Id;
            ViewBag.Partners = _partnerService.All();

            return View(_saleService.All()
                .Where(s => s.Payment == true && s.ShopId == user.ShopId)
                .OrderByDescending(x => x.Date)
                .Take(50)
                .Select(x => new SaleVM()
                {
                    Id = x.Id,
                    Date = x.Date.ToString("dd.MM.yyyy"),
                    Sum = _db.InfoMonies.Where(z => z.SaleId == x.Id).Sum(z => z.Sum),
                    ShopTitle = x.Shop.Title,
                    HasAdditionalProduct = x.SalesProducts
                                               .FirstOrDefault(sp => sp.Additional) != null
                        ? true
                        : false,
                    BuyerTitle = _db.Partners.FirstOrDefault(s => x.PartnerId == s.Id) != null
                        ? _db.Partners.FirstOrDefault(s => x.PartnerId == s.Id).Title
                        : "Обычный покупатель",
                    ProductTitle = _db.SalesProducts
                        .FirstOrDefault(s => s.SaleId == x.Id).Product.Title,
                    PaymentType = _db.InfoMonies.Count(s => s.SaleId == x.Id) > 1
                        ? PaymentType.Mixed
                        : _db.InfoMonies.FirstOrDefault(s => s.SaleId == x.Id) != null
                            ? _db.InfoMonies.FirstOrDefault(s => s.SaleId == x.Id).PaymentType
                            : PaymentType.Cash,
                }).ToList());
        }
    }
}