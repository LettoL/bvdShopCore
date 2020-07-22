using System;
using System.Globalization;
using System.Linq;
using Data.Entities;
using Data.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using WebUI.ViewModels;
using Base.Services.Abstract;
using Data;
using Data.Enums;
using Data.Services;
using Data.ViewModels;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using PostgresData;
using WebUI.Dtos;
using Product = Data.Entities.Product;
using ProductFilterVM = WebUI.ViewModels.ProductFilterVM;
using ProductVM = WebUI.ViewModels.ProductVM;
using User = Data.Entities.User;

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
        private readonly PostgresContext _postgresContext;

        public ProductController(IProductService productService,
            IBaseObjectService<Category> categoryService,
            IShopService shopService,
            IBaseObjectService<SupplyProduct> supplierProductService,
            IBaseObjectService<User> userService,
            IBaseObjectService<BookingProduct> bookingProductService,
            IProductOperationService productOperationService,
            ShopContext db,
            PostgresContext postgresContext)
        {
            _productService = productService;
            _categoryService = categoryService;
            _shopService = shopService;
            _supplyProductService = supplierProductService;
            _userService = userService;
            _bookingProductService = bookingProductService;
            _productOperationService = productOperationService;
            _db = db;
            _postgresContext = postgresContext;
        }

        public IActionResult Index()
        {
            var userName = HttpContext.User.Identity.Name;
            ViewBag.UserId = _userService.All().First(u => u.Login == userName).Id;

            ViewBag.Categories = _categoryService.All();
            ViewBag.Shops = _shopService.All();

            var result = ProductService.GetProductsInStock(_db);

            return View(result);
        }

        public IActionResult AllProducts()
        {
            var userName = HttpContext.User.Identity.Name;
            ViewBag.UserId = _userService.All().First(u => u.Login == userName).Id;
            ViewBag.Categories = _categoryService.All();
            ViewBag.Shops = _shopService.All();

            var result = ProductService.GetAllProducts(_db);

            return View(result);
        }

        [HttpGet]
        public IActionResult Get(int id)
        {
            User user = _userService.All().FirstOrDefault(u => u.Id == id);
            var shopId = user.ShopId;
            
            var bookedProducts = _db.BookingProducts
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
                .Where(x => x.Product.ShopId == shopId)
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
                .ToList();

            return Ok(productsInStock);
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

            var shops = _db.Shops.ToList();
            
            ViewBag.Shops = shops;
            
            ViewBag.IncompleteProducts = _postgresContext.IncompleteProducts
                .Where(x => x.ProductId == id)
                .ToList()
                .Select(x => new IncompleteProductDto()
                {
                    Amount = x.Amount,
                    Comment = x.Comment,
                    Shop = shops.FirstOrDefault(s => s.Id == x.ShopId)?.Title ?? ""
                });

            return View(ProductService.GetProductDetail(_db, id));
        }

        [HttpPost]
        public IActionResult AddIncompleteProduct(int productId, int shopId, int amount, string comment)
        {
            _postgresContext.IncompleteProducts
                .Add(new IncompleteProduct(
                    productId,
                    amount,
                    shopId,
                    comment));

            _postgresContext.SaveChanges();
            
            return RedirectToAction("Detail", new { id = productId});
        }

        [HttpPost]
        public IActionResult Filter(ProductFilterVM productFilterVM)
        {
            var user = _userService.All().First(u => u.Id == productFilterVM.UserId);
            ViewBag.User = user;

            if (user == null)
                RedirectToAction("Index", "Home");
            
            var filter = productFilterVM.ProductFiltrationModel;

            var result = ProductService.GetAllProductsFilter(_db,
                new Data.ViewModels.ProductFilterVM()
                {
                    All = filter.all,
                    CategoryId = filter.categoryId,
                    ShopId = filter.shopId,
                    Title = filter.title
                });
            
            return PartialView(result);
        }

        [HttpPost]
        public IActionResult ProductsInStockFilter(ProductFilterVM productFilterVm)
        {
            var user = _userService.All().First(u => u.Id == productFilterVm.UserId);
            ViewBag.User = user;

            if (user == null)
                RedirectToAction("Index", "Home");
            
            var filter = productFilterVm.ProductFiltrationModel;

            var result = ProductService.GetProductsInStockFilter(_db,
                new Data.ViewModels.ProductFilterVM()
                {
                    All = filter.all,
                    CategoryId = filter.categoryId,
                    ShopId = filter.shopId,
                    Title = filter.title
                });
            
            return PartialView(result);
        }

        [HttpPost]
        public IActionResult ChangePrice([FromBody]ChangePrice changePrice)
        {
            var price = Convert.ToDecimal(changePrice.Price, CultureInfo.CreateSpecificCulture("ru-RU"));
            
            _productOperationService.ChangePrice(changePrice.ProductId, price);

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
