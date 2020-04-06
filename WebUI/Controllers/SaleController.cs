using System;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Base.Services.Abstract;
using Data;
using Data.Entities;
using Data.Enums;
using Data.FiltrationModels;
using Data.Services.Abstract;
using Data.Services.Concrete.Filtration;
using Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebUI.ViewModels;

namespace WebUI.Controllers
{
    [Authorize]
    public class SaleController : Controller
    {
        private readonly ISaleService _saleService;
        private readonly IBaseObjectService<User> _userService;
        private readonly IBaseObjectService<SaleProduct> _saleProductService;
        private readonly IInfoMoneyService _infoMoneyService;
        private readonly IBaseObjectService<Partner> _partnerService;
        private readonly IBaseObjectService<ProductInformation> _productInformationService;
        private readonly IBaseObjectService<SupplyProduct> _supplyProductService;
        private readonly IBaseObjectService<Supplier> _supplierService;
        private readonly IBaseObjectService<Shop> _shopService;
        private readonly IBookingProductInformationService _bookingProductInformationService;
        private readonly IMoneyOperationService _moneyOperationService;
        private readonly ISaleStatisticService _saleStatisticService;
        private readonly ISaleInfoService _saleInfoService;
        private readonly IProductOperationService _productOperationService;
        private readonly ShopContext _db;

        public SaleController(ISaleService saleService, 
            IBaseObjectService<User> userService,
            IBaseObjectService<SaleProduct> saleProductService,
            IInfoMoneyService infoMoneyService,
            IBaseObjectService<Partner> partnerService,
            IBaseObjectService<ProductInformation> productInformationService,
            IBaseObjectService<SupplyProduct> supplyProductService,
            IBaseObjectService<Supplier> supplierService,
            IBaseObjectService<Shop> shopService,
            IMoneyOperationService moneyOperationService,
            ShopContext db,
            ISaleStatisticService saleStatisticService,
            ISaleInfoService saleInfoService,
            IProductOperationService productOperationService,
            IBookingProductInformationService bookingProductInformationService)
        {
            _saleService = saleService;
            _userService = userService;
            _saleProductService = saleProductService;
            _infoMoneyService = infoMoneyService;
            _partnerService = partnerService;
            _productInformationService = productInformationService;
            _supplyProductService = supplyProductService;
            _supplierService = supplierService;
            _db = db;
            _shopService = shopService;
            _moneyOperationService = moneyOperationService;
            _saleStatisticService = saleStatisticService;
            _saleInfoService = saleInfoService;
            _productOperationService = productOperationService;
            _bookingProductInformationService = bookingProductInformationService;
        }

        public IActionResult Index()
        {
            var userName = HttpContext.User.Identity.Name;
            ViewBag.UserId = _userService.All().First(u => u.Login == userName).Id;

            ViewBag.Partners = _partnerService.All();
            ViewBag.Shops = _shopService.All();

            return View(_saleStatisticService.SaleList(_db).ToList());
        }

        [HttpGet]
        public IActionResult Detail(int id)
        {
            var userName = HttpContext.User.Identity.Name;
            var user = _userService.All().First(u => u.Login == userName).Role.ToString();

            ViewBag.Role = user;

            Sale sale = _saleService.All().Select(x => new Sale()
            {
                Id = x.Id,
                Title = x.Title,
                Date = x.Date,
                Sum = _infoMoneyService.All().Where(z => z.SaleId == x.Id).Sum(z => z.Sum),
                Discount = x.Discount,
                Margin = x.Margin,
                Partner = x.Partner,
                PrimeCost = x.PrimeCost,
                Shop = x.Shop,
                SaleType = x.SaleType
            }).First(p => p.Id == id);
          
            ViewBag.SalesProducts = _saleProductService.All().Select(sp => new SaleProduct
            {
                SaleId = sp.SaleId,
                Product = sp.Product,
                Amount = sp.Amount,
                Additional = sp.Additional,
                Cost = sp.Cost
            }).Where(sp => sp.SaleId == sale.Id).ToList();

            ViewBag.PaymentType = _saleInfoService.PaymentType(sale.Id);

            var scores = _infoMoneyService.All().Where(x => x.SaleId == id);

            ViewBag.SaleFinanceDetail = new SaleDetailFinanceVM
            {
                CashScores = scores.Where(x => x.PaymentType == PaymentType.Cash)
                    .Select(x => new SaleDetailFinanceItemVM
                    {
                        MoneyWorkerTitle = x.MoneyWorker.Title,
                        Sum = x.Sum.ToString()
                    }),
                CashlessScores = scores.Where(x => x.PaymentType == PaymentType.Cashless)
                    .Select(x => new SaleDetailFinanceItemVM
                    {
                        MoneyWorkerTitle = x.MoneyWorker.Title,
                        Sum = x.Sum.ToString()
                    })
            };


            return View(sale);
        }

        [HttpPost]
        public IActionResult Filter(SaleFilterVM saleFilterVM)
        {
            var user = _userService.All().First(u => u.Id == saleFilterVM.UserId);
            ViewBag.User = user;
            if (user == null)
                RedirectToAction("Index", "Home");

            var getAllSales = _saleService.Filtration(saleFilterVM.SaleFiltrationModel);

            if (user.Role != Role.Administrator)
                getAllSales = getAllSales.Where(x => x.ShopId == user.ShopId);

            var response = getAllSales
                .OrderByDescending(s => s.Id)
                .Select(s => new SaleVM()
            {
                Id = s.Id,
                Date = s.Date.ToString("dd.MM.yyyy"),
                Sum = s.Sum,
                ShopTitle = s.Shop.Title,
                PaymentType = _db.InfoMonies.Count(x => x.SaleId == s.Id) > 1
                    ? PaymentType.Mixed
                    : _db.InfoMonies.FirstOrDefault(x => x.SaleId == s.Id) != null
                        ? _db.InfoMonies.FirstOrDefault(x => x.SaleId == s.Id).PaymentType
                        : PaymentType.Cash, //Пиздец
                HasAdditionalProduct = s.SalesProducts.FirstOrDefault(sp => sp.Additional) != null
                    ? true
                    : false,
                BuyerTitle = _db.Partners.FirstOrDefault(x => x.Id == s.PartnerId) != null
                    ? _db.Partners.FirstOrDefault(x => s.PartnerId == x.Id).Title
                    : "Обычный покупатель",
                ProductTitle = _db.SalesProducts
                    .FirstOrDefault(z => z.SaleId == s.Id).Product.Title,
                PrimeCost = s.PrimeCost
            }).ToList();

            return PartialView(response);
        }

        [HttpPost]
        public IActionResult GetByMonth([FromBody] int month)
        {

            return Ok(_saleService.All().Where(s => s.Date.Month == month && s.Payment == true)
                .Select(x => new SaleVM
            {
                Id = x.Id,
                Date = x.Date.ToString("dd.MM.yyyy"),
                Sum = x.Sum
            }));
        }

        [HttpPost]
        public IActionResult GetByYear([FromBody] int year)
        {
            return Ok(_saleService.All().Where(s => s.Date.Year == year && s.Payment == true)
                .Select(x => new SaleVM
            {
                Id = x.Id,
                Date = x.Date.ToString("dd.MM.yyyy"),
                Sum = x.Sum
            }));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Sale sale = _saleService.All().FirstOrDefault(x => x.Id == id);
            var informations = _productInformationService.All().Where(x => x.SaleId == sale.Id).ToList();

            foreach (var info in informations)
            {
                var supplyProduct = _supplyProductService.All().FirstOrDefault(x => x.Id == info.SupplyProductId);

                if (info.ForRealization == true)
                {
                    supplyProduct.StockAmount += info.Amount;
                    supplyProduct.RealizationAmount += info.Amount;

                    var supplier = _supplierService.All().FirstOrDefault(x => x.Id == supplyProduct.SupplierId);

                    supplier.Debt -= supplyProduct.ProcurementCost * info.Amount;

                    _supplierService.Update(supplier);
                }
                else if(supplyProduct != null)
                {
                    supplyProduct.StockAmount += info.Amount;
                }

                if(supplyProduct != null)
                _supplyProductService.Update(supplyProduct);
            }

            var infoMoneys = _infoMoneyService.All().Where(x => x.SaleId == sale.Id).ToList();

            foreach (var infoMoney in infoMoneys)
            {
                _infoMoneyService.Delete(infoMoney);
            }

            var salesProducts = _saleProductService.All().Where(x => x.SaleId == sale.Id).ToList();

            foreach (var saleProduct in salesProducts)
            {
                _saleProductService.Delete(saleProduct);
            }

            _saleService.Delete(sale);

            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult BuyersFilter(int value)
        {
            if ( value > 0) // Вернуть продажи с партнерами
            {
                return PartialView(_saleService.All()
                    .Where(s => s.PartnerId == value)
                    .OrderByDescending(s => s.Id)
                    .Select(s => new SaleVM()
                {
                    Id = s.Id,
                    Date = s.Date.ToString("dd.MM.yyyy"),
                    Sum = s.Sum,
                    ShopTitle = s.Shop.Title,
                    PaymentType = _infoMoneyService.All().FirstOrDefault(x => x.SaleId == s.Id).PaymentType,
                    HasAdditionalProduct = s.SalesProducts.FirstOrDefault(sp => sp.Additional) != null
                        ? true
                        : false,
                    BuyerTitle = _partnerService.All().FirstOrDefault(x => s.PartnerId == x.Id).Title
                }));
            }
            else if (value == 0) // Вернуть все продажи
            {
                return PartialView(_saleService.All()
                    .OrderByDescending(s => s.Id)
                    .Select(s => new SaleVM()
                {
                    Id = s.Id,
                    Date = s.Date.ToString("dd.MM.yyyy"),
                    Sum = s.Sum,
                    ShopTitle = s.Shop.Title,
                    PaymentType = _infoMoneyService.All().FirstOrDefault(x => x.SaleId == s.Id).PaymentType,
                    HasAdditionalProduct = s.SalesProducts.FirstOrDefault(sp => sp.Additional) != null
                        ? true
                        : false,
                    BuyerTitle = _partnerService.All().FirstOrDefault(x => s.PartnerId == x.Id) != null
                        ? _partnerService.All().FirstOrDefault(x => s.PartnerId == x.Id).Title
                        : "Обычный покупатель"
                }));
            }
            else //Вернуть продажи с обычными покупателями
            {
                return PartialView(_saleService.All()
                    .Where(s => s.PartnerId == null)
                    .OrderByDescending(s => s.Id)
                    .Select(s => new SaleVM()
                    {
                        Id = s.Id,
                        Date = s.Date.ToString("dd.MM.yyyy"),
                        Sum = s.Sum,
                        ShopTitle = s.Shop.Title,
                        PaymentType = _infoMoneyService.All().FirstOrDefault(x => x.SaleId == s.Id).PaymentType,
                        HasAdditionalProduct = s.SalesProducts.FirstOrDefault(sp => sp.Additional) != null
                            ? true
                            : false,
                        BuyerTitle = "Обычный покупатель"
                    }));
            }
        }

        public IActionResult SalesWithPartners()
        {
            ViewBag.Partners = _db.Partners.ToList();

            return View(_db.Sales
                .Where(s => s.Payment == true && s.PartnerId != null)
                .OrderByDescending(x => x.Id)
                .Select(x => new SaleVM()
                {
                    Id = x.Id,
                    Date = x.Date.ToString("dd.MM.yyyy"),
                    Sum = x.Sum,
                    ShopTitle = x.Shop.Title,
                    HasAdditionalProduct = x.SalesProducts
                                               .FirstOrDefault(sp => sp.Additional) != null
                        ? true
                        : false,
                    BuyerTitle = _db.Partners.FirstOrDefault(s => x.PartnerId == s.Id) != null
                        ? _db.Partners.FirstOrDefault(s => x.PartnerId == s.Id).Title
                        : "Обычный покупатель",
                    Comment = x.Comment
                }).ToList()
                .Select(x => new SaleVM
                {
                    Id = x.Id,
                    Date = x.Date,
                    Sum = x.Sum,
                    ShopTitle = x.ShopTitle,
                    HasAdditionalProduct = x.HasAdditionalProduct,
                    PaymentType = _infoMoneyService.All().Count(s => s.SaleId == x.Id) > 1
                        ? PaymentType.Mixed
                        : _infoMoneyService.All().FirstOrDefault(s => s.SaleId == x.Id) != null
                            ? _infoMoneyService.All().FirstOrDefault(s => s.SaleId == x.Id).PaymentType
                            : PaymentType.Cash, //Пиздец
                    BuyerTitle = x.BuyerTitle,
                    Comment = x.Comment
                }));
        }

        [HttpPost]
        public IActionResult ChangeProcurementCost(int saleId, int productId, decimal procurementCost)
        {
            _saleService.ChangeProductProcurementCost(productId, saleId, procurementCost);

            return RedirectToAction("Index");
        }

        public IActionResult SalePaymentList()
        {
            var userName = HttpContext.User.Identity.Name;
            var user = _userService.All().First(u => u.Login == userName);

            ViewBag.Layout = "~/Views/Shared/AdminLayout.cshtml";

            var infoMoneyQuery = _infoMoneyService.All()
                .Where(x => (x.SaleId != null || x.BookingId != null))
                .OrderByDescending(x => x.Id);

            if (user.Role == Role.Manager)
            {
                infoMoneyQuery = infoMoneyQuery
                    .Where(x => x.Sale.ShopId == user.ShopId)
                    .OrderByDescending(x => x.Id);

                ViewBag.Layout = "~/Views/Shared/ManagerLayout.cshtml";
            }
                

            var response = infoMoneyQuery
                .Select(x => new SalePaymentVM
                {
                    Date = x.Date.ToString("d"),
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
                    ShopId = x.SaleId == null
                        ? x.Booking.ShopId.Value
                        : x.Sale.ShopId,
                    ShopTitle = x.SaleId == null
                        ? x.Booking.Shop.Title
                        : x.Sale.Shop.Title,

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
                    ShopId = x.ShopId,
                    ShopTitle = x.ShopTitle,
                    SaleProducts = x.SaleId != null
                        ? _saleInfoService.GetProductsBySaleId(_db, x.SaleId.Value)
                            .Select(z => new SaleProductItemVM
                            {
                                Title = z.Product.Title,
                                Amount = z.Amount.ToString()
                            })
                        : _bookingProductInformationService.GetBookingProductByBooking(_db, x.BookingId.Value)
                            .Select(z => new SaleProductItemVM
                            {
                                Title = z.Product.Title,
                                Amount = z.Amount.ToString()
                            }),
                });




            return View(response);
        }
    }
}
