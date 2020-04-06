using System.Linq;
using Data.Entities;
using Data.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using WebUI.ViewModels;
using Base.Services.Abstract;
using Data;
using Data.Enums;
using Data.FiltrationModels;
using Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace WebUI.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IBaseObjectService<Category> _categoryService;
        private readonly IShopService _shopService;
        private readonly IBaseObjectService<SupplyProduct> _supplyProductService;
        private readonly IBaseObjectService<User> _userService;
        private readonly IBaseObjectService<BookingProduct> _bookingProductService;
        private readonly IProductOperationService _productOperationService;
        private readonly ShopContext _db;

        public ProductController(IProductService productService,
            IBaseObjectService<Category> categoryService,
            IShopService shopService,
            IBaseObjectService<SupplyProduct> supplierProductService,
            IBaseObjectService<User> userService,
            IBaseObjectService<BookingProduct> bookingProductService,
            IProductOperationService productOperationService,
            ShopContext db)
        {
            _productService = productService;
            _categoryService = categoryService;
            _shopService = shopService;
            _supplyProductService = supplierProductService;
            _userService = userService;
            _bookingProductService = bookingProductService;
            _productOperationService = productOperationService;
            _db = db;
        }

        public IActionResult Index()
        {
            var userName = HttpContext.User.Identity.Name;
            ViewBag.UserId = _userService.All().First(u => u.Login == userName).Id;

            ViewBag.Categories = _categoryService.All();
            ViewBag.Shops = _shopService.All();

            return View(_productService.All()
                .Select(x => new ProductVM()
                {
                    Id = x.Id,
                    Title = x.Title,
                    Cost = x.Cost,
                    Shop = x.Shop,
                    Category = x.Category,
                    Code = x.Code,
                })
                .ToList()
                .Where(x => _supplyProductService.All().Where(s => s.ProductId == x.Id)
                    .Sum(s => s.StockAmount) > 0)
                .OrderBy(x => x.Title)
                .Select(x => new ProductVM()
                {
                    Id = x.Id,
                    Title = x.Title,
                    Amount = _supplyProductService.All().Where(s => s.ProductId == x.Id)
                        .Sum(s => s.StockAmount) - _productService.BookedProducts(_db, x.Id, x.Shop.Id),
                    Cost = x.Cost,
                    Shop = x.Shop,
                    Category = x.Category,
                    Code = x.Code,
                    BookedCount = _productService.BookedProducts(_db, x.Id, x.Shop.Id)
                }).ToList());
        }

        public IActionResult AllProducts()
        {
            var userName = HttpContext.User.Identity.Name;
            ViewBag.UserId = _userService.All().First(u => u.Login == userName).Id;
            ViewBag.Categories = _categoryService.All();
            ViewBag.Shops = _shopService.All();

            return View(_db.Products
                .Include(x => x.Shop)
                .Include(x => x.Category)
                .OrderBy(x => x.Title)
                .ToList()
                .Select(x => new ProductVM()
                {
                    Id = x.Id,
                    Title = x.Title,
                    Amount = _db.SupplyProducts.Where(s => s.ProductId == x.Id)
                        .Sum(s => s.StockAmount) - _productService.BookedProducts(_db, x.Id, x.ShopId),
                    Cost = x.Cost,
                    Shop = x.Shop,
                    Category = x.Category,
                    Code = x.Code,
                    BookedCount = _productService.BookedProducts(_db, x.Id, x.ShopId)
                }).ToList());
        }

        [HttpGet]
        public IActionResult Get(int id)
        {
            User user = _userService.All().FirstOrDefault(u => u.Id == id);

            return Ok(_productService.All()
                .Select(x => new Product()
                {
                    Id = x.Id,
                    Title = x.Title,
                    Cost = x.Cost,
                    Category = x.Category,
                    ShopId = x.ShopId
                })
                .ToList()
                .Where(x => x.ShopId == user.ShopId
                            && (_supplyProductService.All()
                                    .Where(s => s.ProductId == x.Id)
                                    .Sum(s => s.StockAmount) -
                                _bookingProductService.All()
                                    .Where(y => y.ProductId == x.Id
                                                && y.Booking.Status == BookingStatus.Open)
                                    .Sum(y => y.Amount)) > 0)
                .Select(x => new ProductVM()
                {
                    Id = x.Id,
                    Title = x.Title,
                    Cost = x.Cost,
                    Category = x.Category,
                    Amount = _supplyProductService.All()
                        .Where(z => z.ProductId == x.Id)
                        .Where(s => s.Product.ShopId == x.ShopId)
                        .Sum(s => s.StockAmount) - 
                            _bookingProductService.All()
                                .Where(z => z.ProductId == x.Id 
                                            && z.Booking.Status == BookingStatus.Open)
                                .Sum(z => z.Amount)
                }));
        }

        public IActionResult All()
        {
            return Ok(_productService.All().Select(x => new ProductVM()
            {
                Id = x.Id,
                Title = x.Title,
                Cost = x.Cost,
                Category = x.Category,
            }));
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = _categoryService.All();
            ViewBag.Shops = _shopService.All();

            return View();
        }

        [HttpPost]
        public IActionResult Create(Product obj)
        {
            _productService.Create(obj);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Categories = _categoryService.All();
            ViewBag.Shops = _shopService.All();

            return View(_productService.All().Select(x => new Product()
            {
                Id = x.Id,
                Category = x.Category,
                Code = x.Code,
                Cost = x.Cost,
                Title = x.Title,
                Shop = x.Shop,
            }).FirstOrDefault(x => x.Id == id));
        }

        [HttpPost]
        public IActionResult Edit(Product obj)
        {
            _productService.Update(obj);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Detail(int id)
        {
            ProductVM product = _productService.All()
                .Select(x => new Product()
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
                    Category = x.Category,
                    Code = x.Code,
                    Cost = x.Cost,
                    Title = x.Title,
                    Shop = x.Shop,
                    Amount = _supplyProductService.All().Where(s => s.ProductId == x.Id)
                        .Sum(s => s.StockAmount)
                }).First(p => p.Id == id);


            //TODO: Сделать Join один поставщик - один SupplyProduct на отображение
            //Для таба с поставщиками
            ViewBag.SupplierProducts = _supplyProductService.All()
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
            ViewBag.SupplyProduct = _supplyProductService.All()
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

        [HttpPost]
        public IActionResult Filter(ProductFilterVM ProductFilterVM)
        {
            var user = _userService.All().First(u => u.Id == ProductFilterVM.UserId);
            ViewBag.User = user;

            if (user == null)
                RedirectToAction("Index", "Home");

            var getAllProducts = _productService.Filtration(ProductFilterVM.ProductFiltrationModel);
                     
            return PartialView(getAllProducts
                .OrderBy(x => x.Title)
                .Select(x => new ProductVM
                {
                    Id = x.Id,
                    Title = x.Title,
                    Amount = _supplyProductService.All()
                        .Where(s => s.ProductId == x.Id)
                        .Sum(s => s.StockAmount) - _productService.BookedProducts(_db, x.Id, x.ShopId),
                    Cost = x.Cost,
                    Shop = x.Shop,
                    Category = x.Category,
                    Code = x.Code,
                    BookedCount = _productService.BookedProducts(_db, x.Id, x.ShopId)
                }));
        }

        [HttpPost]
        public IActionResult ChangePrice([FromBody]ChangePrice changePrice)
        {
            _productOperationService.ChangePrice(changePrice.ProductId, changePrice.Price);

            return Ok();
        }
       
        [Route("SupplyProduct")]
        [HttpPost]
        public void SupplyProduct(SupplyProductVM obj)
        {
            _productOperationService.Supply(obj);
        }
    }
}
