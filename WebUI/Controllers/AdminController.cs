using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Base.Services.Abstract;
using Data;
using Data.Entities;
using Data.Enums;
using Data.FiltrationModels;
using Data.ModernServices.Abstract;
using Data.Services;
using Data.Services.Abstract;
using Data.ViewModels;
using Domain.Entities;
using Domain.Entities.Olds;
using Domain.Entities.Supplies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using PostgresData;
using WebUI.Dtos;
using WebUI.Dtos.CloseSale;
using WebUI.QueriesHandlers;
using WebUI.ViewModels;
using Manager = Domain.Entities.Sales.Manager;
using Product = Data.Entities.Product;
using ProductFilterVM = Data.ViewModels.ProductFilterVM;
using ProductVM = WebUI.ViewModels.ProductVM;
using Shop = Data.Entities.Shop;
using Supplier = Data.Entities.Supplier;
using User = Data.Entities.User;
using Sale = Data.Entities.Sale;
using SupplyType = Data.Enums.SupplyType;

namespace WebUI.Controllers
{
    //[Authorize]
    public class AdminController : Controller
    {
        private readonly IShopService _shopService;
        private readonly IBaseObjectService<Supplier> _supplierService;
        private readonly IProductService _productService;
        private readonly IBaseObjectService<Category> _categoryService;
        private readonly IInfoMoneyService _infoMoneyService;
        private readonly IInfoProductService _infoProductService;
        private readonly IBaseObjectService<BookingProduct> _bookingProductService;
        private readonly IBaseObjectService<Booking> _bookingService;
        private readonly IBaseObjectService<SaleProduct> _saleProductService;
        private readonly ISaleService _saleService;
        private readonly IBaseObjectService<DeferredSupplyProduct> _deferredSupplyProductService;
        private readonly IBaseObjectService<User> _userService;
        private readonly IBaseObjectService<SaleInformation> _saleInfromationService;
        private readonly IBaseObjectService<ExpenseCategory> _expenseCategoryService;
        private readonly IBaseObjectService<Expense> _expenseService;
        private readonly IFileService _fileService;
        private readonly ShopContext _db;
        private readonly IBaseObjectService<SupplyProduct> _supplyProduct;
        private readonly IBaseObjectService<MoneyWorker> _moneyWorkerService;
        private readonly IBaseObjectService<MoneyTransfer> _moneyTransferService;
        private readonly IBaseObjectService<ProductInformation> _productInformationService;
        private readonly IProductOperationService _productOperationService;
        private readonly IMoneyOperationService _moneyOperationService;
        private readonly IMoneyStatisticService _moneyStatisticService;
        private readonly IBaseObjectService<SupplyHistory> _supplyHistoryService;
        private readonly ISaleInfoService _saleInfoService;
        private readonly IBookingProductInformationService _bookingProductInformationService;
        private readonly ISalesByCategoryService _salesByCategoryService;
        private readonly PostgresContext _postgresContext;

        public AdminController(IShopService shopService,
            IBaseObjectService<Supplier> supplierService,
            IBaseObjectService<Category> categoryService,
            IProductService productService,
            IInfoMoneyService infoMoneyService,
            IInfoProductService infoProductService,
            IBaseObjectService<BookingProduct> bookingProductService,
            IBaseObjectService<Booking> bookingService,
            IBaseObjectService<SaleProduct> saleProductService,
            ISaleService saleService,
            IBaseObjectService<DeferredSupplyProduct> deferredSupplyProductService,
            IBaseObjectService<User> userService,
            IBaseObjectService<SaleInformation> saleInfromationService,
            IBaseObjectService<ExpenseCategory> expenseCategoryService,
            IBaseObjectService<Expense> expenseService,
            ShopContext db,
            IFileService fileService,
            IBaseObjectService<SupplyProduct> supplyProduct,
            IBaseObjectService<MoneyWorker> moneyWorkerService,
            IBaseObjectService<MoneyTransfer> moneyTransferService,
            IBaseObjectService<ProductInformation> productInformationService,
            IProductOperationService productOperationService,
            IMoneyOperationService moneyOperationService,
            IMoneyStatisticService moneyStatisticService,
            IBaseObjectService<SupplyHistory> supplyHistoryService,
            ISaleInfoService saleInfoService,
            IBookingProductInformationService bookingProductInformationService,
            ISalesByCategoryService salesByCategoryService,
            PostgresContext postgresContext)
        {
            _shopService = shopService;
            _supplierService = supplierService;
            _productService = productService;
            _categoryService = categoryService;
            _infoMoneyService = infoMoneyService;
            _infoProductService = infoProductService;
            _bookingService = bookingService;
            _bookingProductService = bookingProductService;
            _saleProductService = saleProductService;
            _saleService = saleService;
            _deferredSupplyProductService = deferredSupplyProductService;
            _userService = userService;
            _saleInfromationService = saleInfromationService;
            _expenseCategoryService = expenseCategoryService;
            _expenseService = expenseService;
            _db = db;
            _fileService = fileService;
            _supplyProduct = supplyProduct;
            _moneyWorkerService = moneyWorkerService;
            _moneyTransferService = moneyTransferService;
            _productInformationService = productInformationService;
            _productOperationService = productOperationService;
            _moneyOperationService = moneyOperationService;
            _moneyStatisticService = moneyStatisticService;
            _supplyHistoryService = supplyHistoryService;
            _saleInfoService = saleInfoService;
            _bookingProductInformationService = bookingProductInformationService;
            _salesByCategoryService = salesByCategoryService;
            _postgresContext = postgresContext;
        }

        [HttpPost]
        public IActionResult Test([FromBody]string test)
        {
          return Ok(test);
        }

        public async Task<IActionResult> Managers()
        {
            var userName = HttpContext.User.Identity.Name;
            if (_userService.All().First(u => u.Login == userName).Role != Role.Administrator)
                return RedirectToAction("Login", "Account");
            
            var saleManagers = await _postgresContext.SaleManagersOld.ToListAsync();

            var salesId = saleManagers.Select(x => x.SaleId).ToList();
            var sales = await _db.Sales
                .Where(x => salesId.Contains(x.Id))
                .Select(x => new
                {
                    SaleId = x.Id,
                    Margin = x.Margin,
                    Sum = x.Sum,
                })
                .ToListAsync();

            var managers = await _postgresContext.Managers
                .Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();

            var salesWithManager = sales.Select(x => new
            {
                SaleId = x.SaleId,
                Margin = x.Margin,
                Sum = x.Sum,
                ManagerId = saleManagers.FirstOrDefault(z => z.SaleId == x.SaleId).ManagerId
            });

            var result = managers.Select(x => new ManagerDto()
            {
                Id = x.Id,
                Name = x.Name,
                Margin = salesWithManager
                    .Where(z => z.ManagerId == x.Id)
                    .Sum(z => z.Margin),
                Sum = salesWithManager
                    .Where(z => z.ManagerId == x.Id)
                    .Sum(z => z.Sum)
            }).ToList();
            
            return View(result);
        }

        [HttpGet]
        public IActionResult CreateManager()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateManager(string name)
        {
            await _postgresContext.Managers.AddAsync(new Manager(name));
            await _postgresContext.SaveChangesAsync();
            
            return RedirectToAction("Managers");
        }
        
        public IActionResult Index()
        {
            var userName = HttpContext.User.Identity.Name;
            if (_userService.All().First(u => u.Login == userName).Role != Role.Administrator)
                return RedirectToAction("Login", "Account");
            
            var saleProductsToday = new List<SaleProductsTodayVM>();

            foreach (var shop in _db.Shops.ToList())
            {
                var salesProductsToday = _db.SalesProducts
                        .Where(x => x.Sale.ShopId == shop.Id 
                                    && x.Sale.Date.DayOfYear == DateTime.Now.DayOfYear 
                                    && x.Sale.Date.Year == DateTime.Now.Year)
                        .Select(sp => new SaleProduct()
                        {
                            Product = sp.Product,
                            Amount = sp.Amount
                        }).ToList();

                saleProductsToday.Add(new SaleProductsTodayVM()
                {
                    ShopTitle = shop.Title,
                    SalesProducts = salesProductsToday.ToList(),
                    Sum = _db.InfoMonies
                        .Where(im => im.Sale.ShopId == shop.Id
                                     && im.Date.DayOfYear == DateTime.Now.DayOfYear
                                     && im.Sale.Date.Year == DateTime.Now.Year)
                        .Sum(im => im.Sum)
                });
            }

            ViewBag.SalesToday = saleProductsToday;
            
            ViewBag.Sum = _moneyStatisticService.DailyProfit(_db);

            ViewBag.Expenses = _moneyStatisticService.Expenses(_db).ToList();

            ViewBag.DefferedSalesFromStock = _saleService.DeferredSalesFromStock(_db).ToList()
                .Select(s => new SaleVM()
                {
                    Id = s.Id,
                    Date = s.Date.ToString("dd.MM.yyyy"),
                    Sum = _db.InfoMonies.Where(x => x.SaleId == s.Id).Sum(x => x.Sum),
                    ShopTitle = s.Shop.Title,
                    ProductTitle = _saleService.ProductTitle(s.Id)
                }).ToList();

            ViewBag.SalesWithOpenPayments = _saleService.SalesWithOpenPayments(_db)
                .Select(s => new SaleVM()
                {
                    Id = s.Id,
                    Date = s.Date.ToString("dd.MM.yyyy"),
                    Sum = s.Sum,
                    ShopTitle = s.Shop.Title,
                    ProductTitle = _saleService.ProductTitle(s.Id)
                }).ToList();

            ViewBag.DeferredPayments = _deferredSupplyProductService.All()
                .Select(d => new DeferredPaymentsVM()
                {
                    Date = d.Date.Value.ToString("dd.MM.yyyy"),
                    Sum = d.SupplyProduct.ProcurementCost * d.SupplyProduct.TotalAmount,
                    SupplierTitle = d.SupplyProduct.Supplier.Title
                }).ToList();

            ViewBag.Balance = _infoMoneyService.Balance();

            ViewBag.MoscowSalePayments = _infoMoneyService.All()
                .Where(x => (x.SaleId != null || x.BookingId != null) 
                            && x.Date.Date == DateTime.Now.Date.Date
                            && (x.Sale.ShopId == 1 || x.Booking.ShopId == 1))
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

                    OperationType = x.SaleId == null
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
                }).ToList()
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
                        ? _saleInfoService.GetProductsBySaleId(_db, x.SaleId.Value)
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
                }); 

            ViewBag.PetersburgSalePayments = _infoMoneyService.All()
                .Where(x => (x.SaleId != null || x.BookingId != null) 
                            && x.Date.Date == DateTime.Now.Date.Date 
                            && (x.Sale.ShopId == 2 || x.Booking.ShopId == 2))
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

                    OperationType = x.SaleId == null
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
                }).ToList()
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
                        ? _saleInfoService.GetProductsBySaleId(_db, x.SaleId.Value)
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
                });

            return View();
        }

        [HttpGet]
        public IActionResult ImportProductExel()
        {
            ViewBag.Shops = _shopService.All();
            ViewBag.Suppliers = _supplierService.All();
            ViewBag.Categories = _categoryService.All();

            return View();
        }

        [HttpPost]
        public IActionResult ImportProductExel(IFormFile file, int shopId, int? supplierId, int categoryId, decimal deliverySum, int realization, DateTime? date)
        {
            BinaryReader b = new BinaryReader(file.OpenReadStream());
            byte[] data = b.ReadBytes(Convert.ToInt32(file.Length));

            string result = Encoding.GetEncoding(1251).GetString(data);
            string[] lines = result.Split('\r');
            var products = new List<ProductVM>();

            decimal additionalCost = 0;
            if (deliverySum != 0)
                additionalCost = Math.Round(deliverySum / lines.Length, 2);

            decimal procurementCost = 0;

            for (var i = 0; i < lines.Length; i++)
            {
                string[] prop = lines[i].Split(';');
                var buf = prop[3].Replace(',', '.');
                int amount = Convert.ToInt32(Convert.ToDecimal(buf));
                var c = prop[5].Replace(',', '.');
                var d = System.Text.RegularExpressions.Regex.Replace(c, @"\s+", "");//c.Replace(" ", string.Empty);
                decimal a;
                if (Decimal.TryParse(d, out a))
                    procurementCost = a;

                products.Add(new ProductVM()
                {
                    Code = prop[1],
                    Title = prop[2],
                    Amount = amount,
                    ProcurementCost = procurementCost
                });
            }

            var supplyHistory = _supplyHistoryService.Create(new SupplyHistory());

            foreach (var p in products)
            {
                var product = _productService.All()
                    .FirstOrDefault(x => x.Title == p.Title && x.ShopId == shopId);

                if (product == null)
                {
                    var productCost = _productService.All()
                                          .FirstOrDefault(x => x.Title == p.Title)?.Cost ?? 0;

                    var createProduct = _productService.Create(new Product()
                    {
                        Code = p.Code,
                        Title = p.Title,
                        ShopId = shopId,
                        CategoryId = categoryId,
                        Cost = productCost
                    });

                    if (supplierId != null && supplierId != 0)
                    {
                        _db.InfoProducts.Add(new InfoProduct()
                        {
                            Amount = p.Amount,
                            Date = DateTime.Now.AddHours(3),
                            Product = createProduct,
                            SupplierId = supplierId,
                            Type = InfoProductType.Supply,
                            ShopId = shopId,
                            SupplyHistoryId = supplyHistory.Id
                        });
                        
                        var supplyProduct = _supplyProduct.Create(new SupplyProduct()
                        {
                            ProductId = createProduct.Id,
                            SupplierId = supplierId,
                            RealizationAmount = (SupplyType)realization == SupplyType.ForRealization ? p.Amount : 0,
                            TotalAmount = p.Amount,
                            AdditionalCost = additionalCost / p.Amount,
                            ProcurementCost = p.ProcurementCost,
                            FinalCost = p.ProcurementCost + (additionalCost / p.Amount),
                            StockAmount = p.Amount,
                            SupplyHistoryId = supplyHistory.Id
                        });

                        if ((SupplyType)realization == SupplyType.DeferredPayment)
                        {
                            _deferredSupplyProductService.Create(new DeferredSupplyProduct()
                            {
                                Date = date,
                                SupplyProductId = supplyProduct.Id
                            });
                        }
                    }
                    else
                    {
                        var supplyProduct = _supplyProduct.Create(new SupplyProduct()
                        {
                            ProductId = createProduct.Id,
                            RealizationAmount = (SupplyType)realization == SupplyType.ForRealization ? p.Amount : 0,
                            TotalAmount = p.Amount,
                            AdditionalCost = additionalCost / p.Amount,
                            ProcurementCost = p.ProcurementCost,
                            FinalCost = p.ProcurementCost + (additionalCost / p.Amount),
                            StockAmount = p.Amount,
                            SupplyHistoryId = supplyHistory.Id
                        });

                         _db.InfoProducts.Add(new InfoProduct()
                        {
                            Amount = p.Amount,
                            Date = DateTime.Now.AddHours(3),
                            Product = createProduct,
                            Type = InfoProductType.Supply,
                            ShopId = shopId,
                            SupplyHistoryId = supplyHistory.Id
                        });

                        if ((SupplyType)realization == SupplyType.DeferredPayment)
                        {
                            _deferredSupplyProductService.Create(new DeferredSupplyProduct()
                            {
                                Date = date,
                                SupplyProductId = supplyProduct.Id
                            });
                        }
                    }
                }
                else
                {
                    if (supplierId != null && supplierId != 0)
                    {
                        _db.InfoProducts.Add(new InfoProduct()
                        {
                            Amount = p.Amount,
                            Date = DateTime.Now.AddHours(3),
                            ProductId = product.Id,
                            SupplierId = supplierId,
                            Type = InfoProductType.Supply,
                            ShopId = shopId,
                            SupplyHistoryId = supplyHistory.Id
                        });

                        var supplyProduct = _supplyProduct.Create(new SupplyProduct()
                        {
                            ProductId = product.Id,
                            SupplierId = supplierId,
                            RealizationAmount = (SupplyType)realization == SupplyType.ForRealization ? p.Amount : 0,
                            TotalAmount = p.Amount,
                            AdditionalCost = additionalCost / p.Amount,
                            ProcurementCost = p.ProcurementCost,
                            FinalCost = p.ProcurementCost + (additionalCost / p.Amount),
                            StockAmount = p.Amount,
                            SupplyHistoryId = supplyHistory.Id
                        });

                        if ((SupplyType)realization == SupplyType.DeferredPayment)
                        {
                            _deferredSupplyProductService.Create(new DeferredSupplyProduct()
                            {
                                Date = date,
                                SupplyProductId = supplyProduct.Id
                            });
                        }
                    }

                    else
                    {
                        _db.InfoProducts.Add(new InfoProduct()
                        {
                            Amount = p.Amount,
                            Date = DateTime.Now.AddHours(3),
                            ProductId = product.Id,
                            Type = InfoProductType.Supply,
                            ShopId = shopId,
                            SupplyHistoryId = supplyHistory.Id
                        });

                        var supplyProduct = _supplyProduct.Create(new SupplyProduct()
                        {
                            ProductId = product.Id,
                            RealizationAmount = (SupplyType)realization == SupplyType.ForRealization ? p.Amount : 0,
                            TotalAmount = p.Amount,
                            AdditionalCost = additionalCost / p.Amount,
                            ProcurementCost = p.ProcurementCost,
                            FinalCost = p.ProcurementCost + (additionalCost / p.Amount),
                            StockAmount = p.Amount,
                            SupplyHistoryId = supplyHistory.Id
                        });

                        if ((SupplyType)realization == SupplyType.DeferredPayment)
                        {
                            _deferredSupplyProductService.Create(new DeferredSupplyProduct()
                            {
                                Date = date,
                                SupplyProductId = supplyProduct.Id
                            });
                        }
                    }
                }
            }

            _db.SaveChanges();

            _postgresContext.SuppliesInfo.Add(new SupplyInfo()
            {
                DateTime = DateTime.Now.AddHours(3),
                Text = result,
                OldSupplyHistoryId = supplyHistory.Id,
                NewImport = false
            });
            _postgresContext.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult SupplyProduct()
        {
            var userName = HttpContext.User.Identity.Name;
            if (_userService.All().First(u => u.Login == userName).Role != Role.Administrator)
                return RedirectToAction("Login", "Account");
            
            ViewBag.Products = _productService.All();
            ViewBag.Suppliers = _supplierService.All();
            ViewBag.Shops = _shopService.All();

            return View();
        }

        [HttpPost]
        public IActionResult SupplyProduct(SupplyProductVM obj)
        {
            obj.Name = _productService.All().FirstOrDefault(x => x.Id == obj.ProductId)?.Title;

            _productOperationService.Supply(obj);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult WriteoffProduct()
        {
            var userName = HttpContext.User.Identity.Name;
            if (_userService.All().First(u => u.Login == userName).Role != Role.Administrator)
                return RedirectToAction("Login", "Account");
            
            ViewBag.Shops = _shopService.All();

            return View();
        }

        [HttpPost]
        public IActionResult WriteoffProduct(int productId, int supplyId, int amount)
        {
            _productOperationService.WriteOff(productId, supplyId, amount);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult SupplierRepayment()
        {
            var userName = HttpContext.User.Identity.Name;
            if (_userService.All().First(u => u.Login == userName).Role != Role.Administrator)
                return RedirectToAction("Login", "Account");
            
            return View(_supplierService.All().Where(s => s.Debt != 0).Select(x => new Supplier
            {
                Id = x.Id,
                Title = x.Title,
                Debt = x.Debt
            }));
        }

        [HttpPost]
        public IActionResult SupplierRepayment(int supId, int moneyWorkerId, decimal sum)
        {
            Supplier supplier = _supplierService.All().First(s => s.Id == supId);
            supplier.Debt -= sum;
            _supplierService.Update(supplier);

            var infoMoney = _infoMoneyService.Create(new InfoMoney()
            {
                Sum = -sum,
                PaymentType = PaymentType.Cashless,
                MoneyWorkerId = moneyWorkerId,
                MoneyOperationType = MoneyOperationType.SupplierRepayment
            });

            _postgresContext.RepaidDebtsOld.Add(
                new RepaidDebtOld(supplier.Id, infoMoney.Id));
            _postgresContext.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Encashment()
        {
            var userName = HttpContext.User.Identity.Name;
            if (_userService.All().First(u => u.Login == userName).Role != Role.Administrator)
                return RedirectToAction("Login", "Account");
            
            return View(_shopService.All().ToList().Select(s => new ShopVM
            {
                Id = s.Id,
                Title = s.Title,
                CashOnHand = _shopService.CashOnHand(_db, s.Id)
            }));
        }

        [HttpPost]
        public IActionResult Encashment(int shopId, decimal sum)
        {
            _infoMoneyService.Create(new InfoMoney
            {
                Sum = -sum,
                MoneyWorkerId = shopId,
                PaymentType = PaymentType.Cash,
                MoneyOperationType = MoneyOperationType.Encashment
            });

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult MoneyHistory()
        {
            var userName = HttpContext.User.Identity.Name;
            if (_userService.All().First(u => u.Login == userName).Role != Role.Administrator)
                return RedirectToAction("Login", "Account");
            
            ViewBag.UserId = _userService.All().FirstOrDefault(u => u.Login == userName).Id;
            ViewBag.Shops = _shopService.All();
            ViewBag.Scores = _db.MoneyWorkers;

            var result = MoneyHistoryHandlers.GetMoneyHistory(_postgresContext, _db);

            return View(result);
        }

        [HttpPost]
        public IActionResult MoneyHistoryFilter(string date1, string date2, int shopId, int userId, int score, int type)
        {
            var user = _userService.All().FirstOrDefault(u => u.Id == userId);

            if (user == null)
                RedirectToAction("Index", "Home");

            var result = MoneyHistoryHandlers.GetMoneyHistory(
                _postgresContext,
                _db,
                new MoneyHistoryFilterQuery(date1, date2, shopId, score, type));

            return PartialView(result);
        }

        [HttpGet]
        public IActionResult ProductHistory()
        {
            var userName = HttpContext.User.Identity.Name;
            if (_userService.All().First(u => u.Login == userName).Role != Role.Administrator)
                return RedirectToAction("Login", "Account");
            
            ViewBag.Shops = _shopService.All();

            return View(_infoProductService.All().Select(ip => new InfoProduct()
            {
                Id = ip.Id,
                Product = ip.Product,
                Shop = ip.Shop,
                Supplier = ip.Supplier,
                Sale = ip.Sale,
                Amount = ip.Amount,
                Date = ip.Date,
                Type = ip.Type
            }).OrderByDescending(ip => ip.Id));
        }

        [HttpPost]
        public IActionResult ProductHistoryFilter(InfoProductFiltrationModel model)
        {
            var infoProductsAll = _infoProductService.Filtration(model);

            return View(infoProductsAll
              .Select(ip => new InfoProduct()
              {
                  Id = ip.Id,
                  Product = ip.Product,
                  Shop = ip.Shop,
                  Supplier = ip.Supplier,
                  Sale = ip.Sale,
                  Amount = ip.Amount,
                  Date = ip.Date,
                  Type = ip.Type
              }).OrderByDescending(ip => ip.Id)
              .ToList()
              .Where(x => model.type == 0 || (int)x.Type == model.type));
        }

        [HttpGet]
        public IActionResult BookingList()
        {
            var userName = HttpContext.User.Identity.Name;
            if (_userService.All().First(u => u.Login == userName).Role != Role.Administrator)
                return RedirectToAction("Login", "Account");
            
            return View(_bookingService.All()
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
        public IActionResult BookingDetail(int id)
        {
            var userName = HttpContext.User.Identity.Name;
            if (_userService.All().First(u => u.Login == userName).Role != Role.Administrator)
                return RedirectToAction("Login", "Account");
            
            ViewBag.BookingProducts = _bookingProductService.All()
                .Where(bp => bp.BookingId == id)
                .Select(bp => new BookingDetailVM()
                {
                    ProductTitle = bp.Product.Title,
                    ProductCode = bp.Product.Code,
                    Amount = bp.Amount,
                });

            return View(_bookingService.All().FirstOrDefault(b => b.Id == id));
        }

        [HttpPost]
        public IActionResult SupplyAnnulment(int id)
        {
            _productService.SupplyAnnulment(id);

            return RedirectToAction("ProductHistory");
        }

        [HttpGet]
        [Route("/Admin/GetProductsByShop/{id}")]
        public async Task<IActionResult> GetProductsByShop(int id)
        {
            /*var products = _db.Products
                .Where(x => x.ShopId == id)
                .ToList()
                .GroupBy(x => x.Title)
                .Select(x => x.FirstOrDefault())
                .ToList();*/

            var products = ProductService.GetProductsInStockFilter(_db, _postgresContext,
                new Data.ViewModels.ProductFilterVM()
                {
                    ShopId = id,
                });

            return Ok(new
            {
                products = products
            });
        }

        [HttpGet]
        [Route("/Admin/GetSupplyProductByProduct/{id}")]
        public async Task<IActionResult> GetSupplyProductByProduct(int id)
        {
            var supplyProduct = await _supplyProduct.All()
                .Where(x => x.ProductId == id && x.StockAmount > 0)
                .ToListAsync();

            return Ok(new
            {
                supplyProducts = supplyProduct
            });
        }

        public IActionResult ChangeProducts()
        {
            var userName = HttpContext.User.Identity.Name;
            if (_userService.All().First(u => u.Login == userName).Role != Role.Administrator)
                return RedirectToAction("Login", "Account");
            
            ViewBag.Shops = _shopService.All();

            return View();
        }

        [HttpPost]
        public IActionResult ChangeProducts(int prevShop, int nextShop, int productId, int supplyProductId, int amount)
        {
            var prevProduct = _productService.All().FirstOrDefault(p => p.Id == productId);

            var prevSupplyProduct = _supplyProduct.All().FirstOrDefault(x => x.Id == supplyProductId);
            prevSupplyProduct.TotalAmount -= amount;
            prevSupplyProduct.StockAmount -= amount;

            var nextProduct = _productService.All()
                .FirstOrDefault(p => p.ShopId == nextShop && p.Title == prevProduct.Title);

            if (nextProduct == null)
                nextProduct = _productService.Create(new Product()
                {
                    Title = prevProduct.Title,
                    Code = prevProduct.Code,
                    CategoryId = prevProduct.CategoryId,
                    ShopId = nextShop,
                    Cost = prevProduct.Cost
                });

            if (prevSupplyProduct.SupplierId == null)
                _supplyProduct.Create(new SupplyProduct()
                {
                    AdditionalCost = prevSupplyProduct.AdditionalCost,
                    ProcurementCost = prevSupplyProduct.ProcurementCost,
                    FinalCost = prevSupplyProduct.FinalCost,
                    ProductId = nextProduct.Id,
                    StockAmount = amount,
                    TotalAmount = amount,
                    RealizationAmount = 0
                });
            else
                _supplyProduct.Create(new SupplyProduct()
                {
                    AdditionalCost = prevSupplyProduct.AdditionalCost,
                    ProcurementCost = prevSupplyProduct.ProcurementCost,
                    FinalCost = prevSupplyProduct.FinalCost,
                    ProductId = nextProduct.Id,
                    StockAmount = amount,
                    TotalAmount = amount,
                    RealizationAmount = prevSupplyProduct.RealizationAmount >= amount
                        ? amount
                        : 0,
                    SupplierId = prevSupplyProduct.SupplierId
                });

            _infoProductService.Create(new InfoProduct()
            {
                Amount = amount,
                Date = DateTime.Now.AddHours(3),
                ProductId = nextProduct.Id,
                ShopId = prevProduct.ShopId,
                Type = InfoProductType.Transfer
            });

            if (prevSupplyProduct.RealizationAmount >= amount)
                prevSupplyProduct.RealizationAmount -= amount;

            _supplyProduct.Update(prevSupplyProduct);

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("/Admin/GetSupplyProductsByProduct/{productId}")]
        public async Task<IActionResult> GetSupplyProductsByProduct(int productId)
        {
            var supplyProducts = await _db.SupplyProducts
                .Where(x => x.ProductId == productId && x.StockAmount > 0)
                .ToListAsync();

            return Ok(supplyProducts);
        }

        public IActionResult Expense()
        {
            var userName = HttpContext.User.Identity.Name;
            if (_userService.All().First(u => u.Login == userName).Role != Role.Administrator)
                return RedirectToAction("Login", "Account");
            
            ViewBag.CategoryExpense = _expenseCategoryService.All();

            return View();
        }

        [HttpPost]
        public IActionResult Expense(ExpenseVM expense)
        {
            _moneyOperationService.Expense(expense.MoneyWorkerId, expense.Sum,
                _moneyOperationService.PaymentTypeByMoneyWorker(expense.MoneyWorkerId),
                expense.ExpenseCategory, expense.Comment);

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("/Admin/GetMoneyWorkers/{value}")]
        public async Task<IActionResult> GetMoneyWorkers(int value)
        {
            if (value == 1) //Получить держателей карт
            {
                var cardKeepers = await _db.CardKeepers
                    .ToListAsync();
                return Ok(cardKeepers);
            }
            if (value == 2) //Получить рассчетные счета
            {
                var calculatedScores = await _db.CalculatedScores
                    .ToListAsync();
                return Ok(calculatedScores);
            }

            if (value == 3) //Получить магазины
            {
                var shops = await _db.Shops.ToListAsync();
                return Ok(shops);
            }

            return BadRequest();
        }

        public IActionResult MoneyTransfer()
        {
            var userName = HttpContext.User.Identity.Name;
            if (_userService.All().First(u => u.Login == userName).Role != Role.Administrator)
                return RedirectToAction("Login", "Account");
            
            return View();
        }

        [HttpPost]
        public IActionResult MoneyTransfer(MoneyTransferVM moneyTransfer)
        {
            _infoMoneyService.MoneyTransfer(moneyTransfer);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult CloseSale(int id)
        {
            var userName = HttpContext.User.Identity.Name;
            if (_userService.All().First(u => u.Login == userName).Role != Role.Administrator)
                return RedirectToAction("Login", "Account");
            
            Sale sale = _saleService.All().FirstOrDefault(s => s.Id == id);

            ViewBag.Suppliers = _supplierService.All();
            var saleInformation = _db.SaleInformations
                .Include(x => x.MoneyWorkerForExpense)
                .Include(x => x.MoneyWorkerForIncome)
                .Include(x => x.MoneyWorkerForCashlessIncome)
                .FirstOrDefault(x => x.SaleId == id);

            ViewBag.SaleInformation = null;
            ViewBag.Rest = sale.Sum;

            var payments = _infoMoneyService.All().Where(x => x.SaleId == sale.Id).Sum(x => x.Sum);

            ViewBag.Rest -= payments;

            if (saleInformation != null)
            {
                ViewBag.SaleInformation = new CloseSaleVM();

                if (saleInformation.MoneyWorkerForIncomeId != null)
                {
                    ViewBag.SaleInformation.MoneyWorkerIdForIncome = saleInformation.MoneyWorkerForIncomeId;
                    ViewBag.SaleInformation.MoneyWorkerTitleForIncome = saleInformation.MoneyWorkerForIncome.Title;
                }

                if (saleInformation.MoneyWorkerForCashlessIncomeId != null)
                {
                    ViewBag.SaleInformation.MoneyWorkerIdForCashlessIncome = saleInformation.MoneyWorkerForCashlessIncomeId;
                    ViewBag.SaleInformation.MoneyWorkerTitleForCashlessIncome = saleInformation.MoneyWorkerForCashlessIncome.Title;
                }

                if (saleInformation.MoneyWorkerForExpenseId != null)
                {
                    ViewBag.SaleInformation.MoneyWorkerIdForExpense = saleInformation.MoneyWorkerForExpenseId;
                    ViewBag.SaleInformation.MoneyWorkerTitleForExpense = saleInformation.MoneyWorkerForExpense.Title;
                }
            }

            var saleProducts = _saleProductService
                .All()
                .Include(x => x.Product)
                .Where(x => x.SaleId == id)
                .ToList();

            ViewBag.SalesProduct = saleProducts
                .Select(x => new SaleProductItem()
                {
                    Id = x.ProductId,
                    Title = x.Product.Title,
                    Amount = x.Amount,
                    ProcurementCost = 0
                }).ToList();
            
            var saleFromStockInfo = _postgresContext.SalesFromStockOld
                .Include(x => x.Products)
                .FirstOrDefault(x => x.SaleId == id);

            if (saleFromStockInfo != null)
            {
                ViewBag.SalesProduct = saleFromStockInfo.Products
                    .Select(x => new SaleProductItem()
                    {
                        Id = x.ProductId,
                        Title = saleProducts
                            .FirstOrDefault(z => z.ProductId == x.ProductId)?
                            .Product?.Title ?? "",
                        Amount = saleProducts
                            .FirstOrDefault(z => z.ProductId == x.ProductId)?
                            .Amount ?? 0,
                        ProcurementCost = x.ProcurementCost
                    }).ToList();
            
                var selectedSupplierId = saleFromStockInfo.SupplierId;

                ViewBag.SelectedSupplier = _db.Suppliers.FirstOrDefault(x => x.Id == selectedSupplierId);  
            }

            return View(sale);
        }

        [HttpPost]
        public IActionResult CloseSale(int saleId, bool realization, int moneyWorkerType, int moneyWorkerId, int moneyWorkerCashlessId, int supplierId, int[] productIds, int[] amounts, decimal[] procurementCosts, decimal AdditionalCost, bool cashExpense)
        {
            var productCosts = productIds.Zip(procurementCosts, (product, cost) => new
            {
                Id = product,
                Cost = cost
            });

            var products = productCosts.Zip(amounts, (product, amount) => new
            {
                Id = product.Id,
                Cost = product.Cost,
                Amount = amount
            });

            decimal productAdditionalCost = AdditionalCost / products.Sum(x => x.Amount);

            foreach (var product in products)
            {
                _productInformationService.Create(new ProductInformation()
                {
                    SaleId = saleId,
                    Amount = product.Amount,
                    FinalCost = product.Cost * product.Amount + productAdditionalCost,
                    ProcurementCost = product.Cost,
                    AdditionalCost = productAdditionalCost,
                    ProductId = product.Id,
                    ForRealization = realization
                });
            }

            decimal finalCost = products.Sum(x => x.Cost * x.Amount) + AdditionalCost;

            if (realization == true)
            {
                var supplier = _supplierService.All().FirstOrDefault(x => x.Id == supplierId);
                supplier.Debt += procurementCosts.Sum();
                _supplierService.Update(supplier);
            }

            _saleService.ClosePostPayment(saleId, moneyWorkerType, moneyWorkerId, moneyWorkerCashlessId, finalCost);

            return RedirectToAction("Index");
        }

        public IActionResult AdditionalProducts()
        {
            var userName = HttpContext.User.Identity.Name;
            if (_userService.All().First(u => u.Login == userName).Role != Role.Administrator)
                return RedirectToAction("Login", "Account");
            
            ViewBag.UserId = _userService.All().FirstOrDefault(u => u.Login == userName).Id;
            ViewBag.Shops = _shopService.All();

            return View(_db.SalesProducts
                .Where(sp => sp.Additional == true)
                .OrderByDescending(sp => sp.Id)
                .Select(sp => new SaleProductVM()
                {
                    Title = sp.Product.Title,
                    Code = sp.Product.Code,
                    AdditionalComment = _db.Sales.FirstOrDefault(x => x.Id == sp.SaleId).AdditionalComment,
                    Date = _db.Sales.FirstOrDefault(x => x.Id == sp.SaleId).Date.ToString("dd.MM.yyyy"),
                    Amount = sp.Amount,
                    Cost = sp.Product.Cost,
                    ShopTitle = sp.Sale.Shop.Title,
                    SaleNumber = sp.SaleId
                })
            );
        }

        [HttpPost]
        public IActionResult AdditionalProductSearch(string title, int userId)
        {
            var user = _userService.All().First(u => u.Id == userId);
            if (user == null)
                RedirectToAction("Index", "Home");

            return PartialView(_saleProductService.All()
                .Where(sp => sp.Additional && sp.Product.Title.Contains(title))
                .Select(sp => new SaleProductVM()
                {
                    Title = sp.Product.Title,
                    Code = sp.Product.Code,
                    Amount = sp.Amount,
                    Cost = sp.Product.Cost,
                    ShopTitle = sp.Sale.Shop.Title,
                    SaleNumber = sp.SaleId
                })
            );
        }

        [HttpPost]
        public IActionResult AdditionalProductFilter(int shopId, int userId)
        {
            var user = _userService.All().First(u => u.Id == userId);
            if (user == null)
                RedirectToAction("Index", "Home");

            var getAllProducts = _saleProductService.All().Where(p => p.Additional);

            if (shopId != 0)
            {
                getAllProducts = getAllProducts.Where(p => p.Product.ShopId == shopId && p.Additional);
            }

            return PartialView(getAllProducts
                .Select(sp => new SaleProductVM()
                {
                    Title = sp.Product.Title,
                    Code = sp.Product.Code,
                    Amount = sp.Amount,
                    Cost = sp.Product.Cost,
                    ShopTitle = sp.Sale.Shop.Title,
                    SaleNumber = sp.SaleId
                }));
        }

        [HttpGet]
        public IActionResult Replenishment()
        {
            var userName = HttpContext.User.Identity.Name;
            if (_userService.All().First(u => u.Login == userName).Role != Role.Administrator)
                return RedirectToAction("Login", "Account");
            
            return View();
        }

        [HttpPost]
        public IActionResult Replenishment(int moneyWorkerId, decimal sum)
        {
            var moneyWorker = _moneyWorkerService.All().FirstOrDefault(x => x.Id == moneyWorkerId);

            _infoMoneyService.Create(new InfoMoney()
            {
                MoneyWorkerId = moneyWorkerId,
                Sum = sum,
                PaymentType = moneyWorker is Shop
                    ? PaymentType.Cash
                    : PaymentType.Cashless,
                MoneyOperationType = MoneyOperationType.Replenishment
            });

            return RedirectToAction("Index");
        }

        public IActionResult CloseDefferedSale(int id)
        {
            var userName = HttpContext.User.Identity.Name;
            if (_userService.All().First(u => u.Login == userName).Role != Role.Administrator)
                return RedirectToAction("Login", "Account");
            
            Sale sale = _saleService.All().FirstOrDefault(s => s.Id == id);

            ViewBag.Suppliers = _supplierService.All();

            var saleProducts = _saleProductService
                .All()
                .Include(x => x.Product)
                .Where(x => x.SaleId == id)
                .ToList();

            ViewBag.SalesProduct = saleProducts
                .Select(x => new SaleProductItem()
                {
                    Id = x.ProductId,
                    Title = x.Product.Title,
                    Amount = x.Amount,
                    ProcurementCost = 0
                });

            var saleFromStockInfo = _postgresContext.SalesFromStockOld
                .Include(x => x.Products)
                .FirstOrDefault(x => x.SaleId == id);

            if (saleFromStockInfo != null)
            {
                ViewBag.SalesProduct = saleFromStockInfo.Products
                    .Join(saleProducts,
                        sp => sp.ProductId,
                        p => p.ProductId,
                        (sp, p) => new SaleProductItem()
                        {
                            Id = p.ProductId,
                            Title = p.Product.Title,
                            Amount = p.Amount,
                            ProcurementCost = sp.ProcurementCost
                        }).ToList();

                var selectedSupplierId = saleFromStockInfo.SupplierId;

                ViewBag.SelectedSupplier = _db.Suppliers.FirstOrDefault(x => x.Id == selectedSupplierId);
            }

            return View(sale);
        }

        [HttpPost]
        public IActionResult CloseDefferedSale(int id, bool realization, decimal procurementCost, int supplierId, int[] productIds, int[] amounts, decimal AdditionalCost, decimal[] procurementCosts)
        {
            var productCosts = productIds.Zip(procurementCosts, (product, cost) => new
            {
                Id = product,
                Cost = cost
            });

            var products = productCosts.Zip(amounts, (product, amount) => new
            {
                Id = product.Id,
                Cost = product.Cost,
                Amount = amount
            });

            decimal productAdditionalCost = AdditionalCost / products.Sum(x => x.Amount);

            foreach (var product in products)
            {
                _productInformationService.Create(new ProductInformation()
                {
                    SaleId = id,
                    Amount = product.Amount,
                    FinalCost = product.Cost * product.Amount + productAdditionalCost,
                    AdditionalCost = productAdditionalCost,
                    ProcurementCost = product.Cost,
                    ProductId = product.Id,
                    ForRealization = realization
                });
            }

            decimal finalCost = products.Sum(x => x.Cost * x.Amount) + AdditionalCost;

            var sale = _saleService.All().FirstOrDefault(x => x.Id == id);

            decimal paymentsCash = _infoMoneyService.All().Where(x => x.SaleId == sale.Id && x.PaymentType == PaymentType.Cash).Sum(x => x.Sum);
            decimal paymentsCashless = _infoMoneyService.All().Where(x => x.SaleId == sale.Id && x.PaymentType == PaymentType.Cashless).Sum(x => x.Sum);

            sale.Margin = sale.Sum - finalCost;
            sale.PrimeCost = finalCost;
            sale.Payment = true;
            sale.Sum = paymentsCash + paymentsCashless;

            if (realization == true)
            {
                var supplier = _supplierService.All().FirstOrDefault(x => x.Id == supplierId);
                supplier.Debt += procurementCosts.Sum();
                _supplierService.Update(supplier);
            }

            var saleInformation = _saleInfromationService.All().FirstOrDefault(x => x.SaleId == sale.Id);
            _saleInfromationService.Delete(saleInformation);

            _saleService.Update(sale);

            return RedirectToAction("Index");
        }

        public IActionResult ExpenseList()
        {
            var userName = HttpContext.User.Identity.Name;
            if (_userService.All().First(u => u.Login == userName).Role != Role.Administrator)
                return RedirectToAction("Login", "Account");
            
            var expenses = _expenseService
                    .All()
                    .Include(x => x.InfoMoney)
                    .Include(x => x.InfoMoney.MoneyWorker)
                    .Include(x => x.ExpenseCategory)
                    .OrderByDescending(x => x.Id);

            ViewBag.UserId = _userService.All().FirstOrDefault(u => u.Login == userName).Id;
            ViewBag.Categories = _expenseCategoryService.All();
            ViewBag.Scores = _moneyWorkerService.All();
            ViewBag.ExpenseSum = expenses.Sum(x => x.InfoMoney.Sum);

            return View(expenses.Take(300));
        }

        [HttpPost]
        public IActionResult ExpenseListFilter(int category, string date1, string date2, int userId, int score)
        {
            var user = _userService.All().FirstOrDefault(u => u.Id == userId);
            ViewBag.User = user;

            if (user == null)
                RedirectToAction("Index", "Home");

            var result = _expenseService.All();

            if (category > 0)
                result = result.Where(x => x.ExpenseCategoryId == category);

            if (date1 != null)
            {
                Console.WriteLine(date1);
                var buf = date1.Split('.');
                var date = new DateTime(
                    Convert.ToInt32(buf[2]),
                    Convert.ToInt32(buf[1]),
                    Convert.ToInt32(buf[0]));
                
                result = result.Where(x => x.InfoMoney.Date >= date);
            }

            if (date2 != null)
            {
                Console.WriteLine(date1);
                var buf = date2.Split('.');
                var date = new DateTime(
                    Convert.ToInt32(buf[2]),
                    Convert.ToInt32(buf[1]),
                    Convert.ToInt32(buf[0]));
                
                result = result.Where(x => x.InfoMoney.Date <= date.AddDays(1));
            }

            if (score > 0)
                result = result.Where(x => x.InfoMoney.MoneyWorkerId == score);

            result = result
                .Include(x => x.ExpenseCategory)
                .Include(x => x.InfoMoney)
                .Include(x => x.InfoMoney.MoneyWorker)
                .OrderByDescending(x => x.Id);

            ViewBag.ExpenseSum = result.Sum(x => x.InfoMoney.Sum);

            return PartialView(result);
        }

        public IActionResult ExportFile()
        {
            var userName = HttpContext.User.Identity.Name;
            if (_userService.All().First(u => u.Login == userName).Role != Role.Administrator)
                return RedirectToAction("Login", "Account");
            
            return View(_shopService.All());
        }

        [HttpPost]
        public FileResult ExportProduct(int id)
        {
            var fileName = "ExportProducts_" + id + "_" + DateTime.Now.AddHours(3).ToString("dd/MM/yyyy_hh/mm/ss");

            try
            {
                _fileService.ExportProducts(_db, id, fileName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            string path = "wwwroot/ExportFiles/" + fileName + ".csv";
            // Объект Stream
            FileStream fs = new FileStream(path, FileMode.Open);
            string file_type = "text/csv";
            string file_name = fileName + ".csv";
            return File(fs, file_type, file_name);
        }

        public IActionResult MoneyWorkerBalanceInfo()
        {
            var userName = HttpContext.User.Identity.Name;
            if (_userService.All().First(u => u.Login == userName).Role != Role.Administrator)
                return RedirectToAction("Login", "Account");
            
            var moneyWorkers = _moneyWorkerService.All();
            var today = DateTime.Now.AddHours(3).Date;
            var result = new List<MoneyWorkerInfoVM>();

            foreach (var worker in moneyWorkers)
            {
                result.Add(new MoneyWorkerInfoVM()
                {
                    Title = worker.Title,
                    MorningCash = _infoMoneyService.All()
                        .Where(x => x.MoneyWorkerId == worker.Id && x.Date < today)
                        .Sum(x => x.Sum),
                    EveningCash = _infoMoneyService.All()
                        .Where(x => x.MoneyWorkerId == worker.Id && x.Date < today.AddDays(1))
                        .Sum(x => x.Sum)
                });
            }

            return View(result);
        }

        [HttpPost]
        public IActionResult MoneyWorkerBalanceInfoResult(DateTime date)
        {
            var moneyWorkers = _moneyWorkerService.All();
            var result = new List<MoneyWorkerInfoVM>();

            foreach (var worker in moneyWorkers)
            {
                result.Add(new MoneyWorkerInfoVM()
                {
                    Title = worker.Title,
                    MorningCash = _infoMoneyService.All()
                        .Where(x => x.MoneyWorkerId == worker.Id && x.Date < date)
                        .Sum(x => x.Sum),
                    EveningCash = _infoMoneyService.All()
                        .Where(x => x.MoneyWorkerId == worker.Id && x.Date < date.AddDays(1))
                        .Sum(x => x.Sum)
                });
            }

            return PartialView(result);
        }

        public IActionResult MoneyHistoryDetail(int id)
        {
            var userName = HttpContext.User.Identity.Name;
            if (_userService.All().First(u => u.Login == userName).Role != Role.Administrator)
                return RedirectToAction("Login", "Account");
            
            var infoMoney = _infoMoneyService.Get(id);

            switch (infoMoney.MoneyOperationType)
            {
                case MoneyOperationType.Booking:
                    return RedirectToAction("BookingDetail", new { id = infoMoney.BookingId });
                case MoneyOperationType.Sale:
                    return RedirectToAction("Detail", "Sale", new { id = infoMoney.SaleId });
                case MoneyOperationType.Expense:
                    return RedirectToAction("ExpenseDetail", new { id = infoMoney.Id });
                case MoneyOperationType.Transfer:
                    return RedirectToAction("MoneyTransferDetail", new { id = infoMoney.Id });
            }

            return RedirectToAction("MoneyHistory");
        }

        public IActionResult ExpenseDetail(int id)
        {
            var expense = _expenseService.All()
                .Include(x => x.ExpenseCategory)
                .Include(x => x.InfoMoney)
                .Include(x => x.InfoMoney.MoneyWorker)
                .FirstOrDefault(x => x.InfoMoneyId == id);

            return View(expense);
        }

        public IActionResult MoneyTransferDetail(int id)
        {
            var userName = HttpContext.User.Identity.Name;
            if (_userService.All().First(u => u.Login == userName).Role != Role.Administrator)
                return RedirectToAction("Login", "Account");
            
            var moneyTransfer = _moneyTransferService.All()
                .Include(x => x.PrevInfoMoney)
                .Include(x => x.NextInfoMoney)
                .Include(x => x.PrevInfoMoney.MoneyWorker)
                .Include(x => x.NextInfoMoney.MoneyWorker)
                .FirstOrDefault(x => x.PrevInfoMoneyId == id || x.NextInfoMoneyId == id);

            return View(moneyTransfer);
        }

        public IActionResult SalesByCategories()
        {
            var userName = HttpContext.User.Identity.Name;
            if (_userService.All().First(u => u.Login == userName).Role != Role.Administrator)
                return RedirectToAction("Login", "Account");
            
            var fromDate = new DateTime(
                DateTime.Now.AddHours(3).Year,
                DateTime.Now.AddHours(3).Month,
                1);

            var filtrationModelForMoscow = new SaleFiltrationModel()
            {
                shopId = 1, //Id магазина Москвы
                startDate = fromDate
            };

            var filtrationModelForPetersburg = new SaleFiltrationModel()
            {
                shopId = 2, //Id магизина Санкт-Петербурга
                startDate = fromDate
            };
            
            var filtrationModelForSamara = new SaleFiltrationModel()
            {
                shopId = 27,
                startDate = fromDate
            };
            
            var filtrationModelForMoscowSever = new SaleFiltrationModel()
            {
                shopId = 29,
                startDate = fromDate
            };

            var filtrationModelForPartners = new SaleFiltrationModel()
            {
                startDate = fromDate,
                buyer = -1
            };

            var filtrationModelForRF = new SaleFiltrationModel()
            {
                startDate = fromDate,
                forRF = true
            };

            var saleProducts = _db.SalesProducts
                .Include(x => x.Product.Category)
                .Include(x => x.Sale)
                .Where(x => x.Sale.Payment == true
                            && x.Sale.Date.Date >= fromDate)
                .ToList();

            var productInfromations = _db.ProductInformations
                .Include(x => x.Product)
                .Include(x => x.Sale)
                .Where(x => x.Product != null && x.Sale != null
                            && x.Sale.Date.Date >= fromDate
                            && x.Sale.Payment == true)
                .ToList();

            var result = _salesByCategoryService.SaleByCategory(
                _db,
                saleProducts,
                productInfromations)
                .ToList();

            ViewBag.Totals = new SalesByCategoriesTotalsVM
            {
                SumSalesByMoscow = result.Sum(x => x.SalesByMoscow),
                SumSalesByPetersburg = result.Sum(x => x.SalesByPetersburg),
                SumSalesBySamara = result.Sum(x => x.SalesBySamara),
                SumSalesByMoscowSever = result.Sum(x => x.SalesByMoscowSever),
                SumChecksByMoscow = _saleService.Filtration(filtrationModelForMoscow).Count(),
                SumChecksByPetersburg = _saleService.Filtration(filtrationModelForPetersburg).Count(),
                SumChecksBySamara = _saleService.Filtration(filtrationModelForSamara).Count(),
                SumChecksByMoscowSever = _saleService.Filtration(filtrationModelForMoscowSever).Count(),
                SumChecksByPartners = _saleService.Filtration(filtrationModelForPartners).Count(),
                SumChecksByRF = _saleService.Filtration(filtrationModelForRF).Count(),
                SumMargin = result.Sum(x => x.Margin),
                SumForRussianFederation = result.Sum(x => x.ForRussianFederation),
                SumPartnerSales = result.Sum(x => x.PartnerSales),
                SumTurnOver = result.Sum(x => x.TurnOver),
                SumTurnOverMoscow = result.Sum(x => x.TurnOverMoscow),
                SumTurnOverPetersburg = result.Sum(x => x.TurnOverPetersburg),
                SumTurnOverSamara = result.Sum(x => x.TurnOverSamara),
                SumTurnOverMoscowSever = result.Sum(x => x.TurnOverMoscowSever),
                SumTurnOverRF = result.Sum(x => x.TurnOverRF),
                SumTurnOverPartner = result.Sum(x => x.TurnOverPartner),
                SumMarginMoscow = result.Sum(x => x.MarginMoscow),
                SumMarginPetersburg = result.Sum(x => x.MarginPetersburg),
                SumMarginSamara = result.Sum(x => x.MarginSamara),
                SumMarginMoscowSever = result.Sum(x => x.MarginMoscowSever),
                SumMarginPartner = result.Sum(x => x.MarginPartner),
                SumMarginRF = result.Sum(x => x.MarginRF)
            };

            ViewBag.fromDate = fromDate.ToString("dd.MM.yyyy");

            ViewBag.Managers = _postgresContext.Managers
                .Select(x => new ManagerDto()
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();

            return View(result);
        }

        [HttpPost]
        public IActionResult SalesByCategoriesFilter(string fromDate, string forDate, int managerId)
        {
            DateTime? fromD = null;
            DateTime? forD = null;

            if (fromDate != null)
            {
                var buf = fromDate.Split('.');
                fromD = new DateTime(
                    Convert.ToInt32(buf[2]),
                    Convert.ToInt32(buf[1]),
                    Convert.ToInt32(buf[0]));
            }

            if (forDate != null)
            {
                var buf = forDate.Split('.');
                forD = new DateTime(
                    Convert.ToInt32(buf[2]),
                    Convert.ToInt32(buf[1]),
                    Convert.ToInt32(buf[0]));
            }
            
            Console.WriteLine(fromD.ToString());
            Console.WriteLine("---------");
            Console.WriteLine(forD.ToString());
            Console.WriteLine("---------");
            
            var saleProducts = _saleProductService.All()
                .Include(x => x.Product.Category)
                .Include(x => x.Sale)
                .Where(x => x.Sale.Payment == true);

            var productInfromations = _db.ProductInformations
                .Include(x => x.Product)
                .Include(x => x.Sale)
                .Where(x => x.Product != null && x.Sale != null
                            && x.Sale.Payment == true);

            var filtrationModelForMoscow = new SaleFiltrationModel()
            {
                shopId = 1, //Id магазина Москвы
                startDate = fromD,
                endDate = forD,
            };

            var filtrationModelForPetersburg = new SaleFiltrationModel()
            {
                shopId = 2, //Id магизина Санкт-Петербурга
                startDate = fromD,
                endDate = forD
            };
            
            var filtrationModelForSamara = new SaleFiltrationModel()
            {
                shopId = 27,
                startDate = fromD,
                endDate = forD
            };
            
            var filtrationModelForMoscowSever = new SaleFiltrationModel()
            {
                shopId = 29,
                startDate = fromD,
                endDate = forD
            };

            var filtrationModelForPartners = new SaleFiltrationModel()
            {
                startDate = fromD,
                endDate = forD,
                buyer = -1
            };

            var filtrationModelForRF = new SaleFiltrationModel()
            {
                startDate = fromD,
                endDate = forD,
                forRF = true
            };

            if (managerId != 0)
            {
                var salesId = _postgresContext.SaleManagersOld
                    .Where(x => x.ManagerId == managerId)
                    .Select(x => x.SaleId)
                    .ToList();

                saleProducts = saleProducts.Where(x => salesId.Contains(x.SaleId));
                productInfromations = productInfromations.Where(x => salesId.Contains(x.SaleId));
            }

            if (fromDate != null)
            {
                saleProducts = saleProducts.Where(x => x.Sale.Date.Date >= fromD);
                productInfromations = productInfromations.Where(x => x.Sale.Date.Date >= fromD);
            }

            if (forDate != null)
            {
                saleProducts = saleProducts.Where(x => x.Sale.Date.Date <= forD);
                productInfromations = productInfromations.Where(x => x.Sale.Date.Date <= forD);
            }

            var result = _salesByCategoryService.SaleByCategory(
                _db,
                saleProducts.ToList(),
                productInfromations.ToList());

            ViewBag.Totals = new SalesByCategoriesTotalsVM
            {
                SumSalesByMoscow = result.Sum(x => x.SalesByMoscow),
                SumSalesByMoscowSever = result.Sum(x => x.SalesByMoscowSever),
                SumSalesByPetersburg = result.Sum(x => x.SalesByPetersburg),
                SumSalesBySamara = result.Sum(x => x.SalesBySamara),
                SumChecksByMoscow = _saleService.Filtration(filtrationModelForMoscow).Count(),
                SumChecksByMoscowSever = _saleService.Filtration(filtrationModelForMoscowSever).Count(),
                SumChecksByPetersburg = _saleService.Filtration(filtrationModelForPetersburg).Count(),
                SumChecksBySamara = _saleService.Filtration(filtrationModelForSamara).Count(),
                SumChecksByPartners = _saleService.Filtration(filtrationModelForPartners).Count(),
                SumChecksByRF = _saleService.Filtration(filtrationModelForRF).Count(),
                SumMargin = result.Sum(x => x.Margin),
                SumForRussianFederation = result.Sum(x => x.ForRussianFederation),
                SumPartnerSales = result.Sum(x => x.PartnerSales),
                SumTurnOver = result.Sum(x => x.TurnOver),
                SumTurnOverMoscow = result.Sum(x => x.TurnOverMoscow),
                SumTurnOverMoscowSever = result.Sum(x => x.TurnOverMoscowSever),
                SumTurnOverPetersburg = result.Sum(x => x.TurnOverPetersburg),
                SumTurnOverSamara = result.Sum(x => x.TurnOverSamara),
                SumTurnOverRF = result.Sum(x => x.TurnOverRF),
                SumTurnOverPartner = result.Sum(x => x.TurnOverPartner),
                SumMarginMoscow = result.Sum(x => x.MarginMoscow),
                SumMarginMoscowSever = result.Sum(x => x.MarginMoscowSever),
                SumMarginPetersburg = result.Sum(x => x.MarginPetersburg),
                SumMarginSamara = result.Sum(x => x.MarginSamara),
                SumMarginPartner = result.Sum(x => x.MarginPartner),
                SumMarginRF = result.Sum(x => x.MarginRF)
            };

            ViewBag.fromDate = fromDate != null ? fromDate : null;
            ViewBag.forDate = forDate != null ? forDate : null;
            ViewBag.ManagerId = managerId;

            return PartialView(result);
        }

        [HttpGet]
        public IActionResult SalesByCategoriesDetail(string fromDate, string forDate,
            SalesByCategoriesFilterType type, int categoryId, int managerId)
        {
            var userName = HttpContext.User.Identity.Name;
            if (_userService.All().First(u => u.Login == userName).Role != Role.Administrator)
                return RedirectToAction("Login", "Account");
            
            ViewBag.CategoryTitle = _categoryService.Get(categoryId).Title;

            DateTime? fromDateFilter = null;

            if (fromDate != null)
                fromDateFilter = DateTime.Parse(fromDate, CultureInfo.CreateSpecificCulture("ru-RU"));

            DateTime? forDateFilter = null;

            if (forDate != null)
                forDateFilter = DateTime.Parse(forDate, CultureInfo.CreateSpecificCulture("ru-RU"));

            var sales = _saleService.All().Where(x => x.Payment == true);

            if (fromDateFilter != null)
                sales = _saleService.All().Where(x => x.Date.Date >= fromDateFilter);

            if (forDateFilter != null)
                sales = sales.Where(x => x.Date.Date <= forDateFilter);
            
            if (managerId != 0)
            {
                var salesIds = _postgresContext.SaleManagersOld
                    .Where(x => x.ManagerId == managerId)
                    .Select(x => x.SaleId)
                    .ToList();

                sales = sales.Where(x => salesIds.Contains(x.Id));
            }

            if (type == SalesByCategoriesFilterType.Moscow)
                sales = sales.Where(x => x.ShopId == 1 && x.PartnerId == null && x.ForRussian == false);

            if (type == SalesByCategoriesFilterType.Piter)
                sales = sales.Where(x => x.ShopId == 2 && x.PartnerId == null && x.ForRussian == false);

            if (type == SalesByCategoriesFilterType.Samara)
                sales = sales.Where(x => x.ShopId == 27 && x.PartnerId == null && x.ForRussian == false);

            if (type == SalesByCategoriesFilterType.MoscowSever)
                sales = sales.Where(x => x.ShopId == 29 && x.PartnerId == null && x.ForRussian == false);
            
            if (type == SalesByCategoriesFilterType.ForRF)
                sales = sales.Where(x => x.ForRussian == true);

            if (type == SalesByCategoriesFilterType.Partner)
                sales = sales.Where(x => x.PartnerId != null && x.ForRussian == false);

            sales = sales
                .Include(x => x.SalesProducts).ThenInclude(x => x.Product)
                .Include(x => x.Partner)
                .Where(x => x.SalesProducts.Any(z => z.Product.CategoryId == categoryId));

            var infoMoneys = _db.InfoMonies.ToList();

            var result = sales.Select(x => new SaleListVM()
                {
                    Id = x.Id,
                    Date = x.Date.ToString("dd.MM.yyyy"),
                    ShopTitle = x.Shop.Title,
                    PrimeCost = x.PrimeCost,
                    BuyerTitle = x.PartnerId != null
                        ? x.Partner.Title
                        : "Обычный покупатель",
                    SalesProducts = x.SalesProducts,
                })
                .ToList()
                .Select(x => new SaleListVM()
                {
                    Id = x.Id,
                    Date = x.Date,
                    Sum = infoMoneys.Where(z => z.SaleId == x.Id).Sum(z => z.Sum),
                    PrimeCost = x.PrimeCost,
                    ShopTitle = x.ShopTitle,
                    HasAdditionalProduct = x.SalesProducts.Any(x => x.Additional),
                    BuyerTitle = x.BuyerTitle,
                    ProductTitle = x.SalesProducts.FirstOrDefault()?.Product.Title ?? "",
                    PaymentType = _saleInfoService.PaymentType(x.Id, infoMoneys),
                }).OrderByDescending(x => x.Id)
                .ToList();
            
            return View(result);
        }
    }
}

