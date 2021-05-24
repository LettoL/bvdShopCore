using System.Collections.Generic;
using System.Linq;
using Data.Entities;
using Data.Enums;
using Data.ViewModels;
using Microsoft.EntityFrameworkCore;
using PostgresData;

namespace Data.Services
{
    public static class ProductService
    {
        public static ICollection<AllProductsDTO> GetAllProducts(ShopContext db)
        {
            var result = db.Test1s.FromSqlRaw(
                   "SELECT Products.Id, Products.Code, Products.Cost, Products.Title, sp.StockAmount, sp.PrimeCost, bp.BookingAmount, Products.CategoryId, Categories.Title as CategoryTitle, Products.ShopId, MoneyWorkers.Title as ShopTitle FROM Products LEFT JOIN (SELECT ProductId, SUM(StockAmount) as StockAmount, SUM(StockAmount * ProcurementCost) as PrimeCost FROM SupplyProducts WHERE StockAmount > 0 GROUP BY ProductId) as sp ON Id = sp.ProductId LEFT JOIN (SELECT ProductId, Sum(Amount) as BookingAmount FROM BookingProducts INNER JOIN Bookings ON BookingId = Bookings.Id AND Bookings.Status = 1 GROUP BY ProductId) as bp ON Id = bp.ProductId INNER JOIN Categories ON CategoryId = Categories.Id INNER JOIN MoneyWorkers ON ShopId = MoneyWorkers.Id")
               .ToList()
               .Select(x => new AllProductsDTO()
               {
                   Id = x.Id,
                   Title = x.Title,
                   Amount = x.StockAmount ?? 0 - x.BookingAmount ?? 0,
                   Cost = x.Cost ?? 0,
                   ShopId = x.ShopId,
                   ShopTitle = x.ShopTitle,
                   CategoryId = x.CategoryId,
                   CategoryTitle = x.CategoryTitle,
                   Code = x.Code,
                   BookedCount = x.BookingAmount ?? 0,
                   PrimeCost = x.PrimeCost ?? 0
               })
               .OrderBy(x => x.Title)
               .ToList();

            return result;
        }

        public static ICollection<AllProductsVM> GetAllProductsInStock(ShopContext db)
        {
            var bookingProductsAmount = db.BookingProducts
                .Where(x => x.Booking.Status == BookingStatus.Open)
                .Select(x => new
                {
                    ProductId = x.ProductId,
                    Amount = x.Amount
                }).ToList();

            var productsInStock = db.SupplyProducts
                .Select(x => new
                {
                    ProductId = x.ProductId,
                    Amount = x.StockAmount,
                    PrimeCost = x.ProcurementCost
                }).ToList();
            
            var result = db.Products
                .Include(x => x.Shop)
                .Include(x => x.Category)
                .OrderBy(x => x.Title)
                .ToList()
                .Select(x => new AllProductsVM()
                {
                    Id = x.Id,
                    Title = x.Title,
                    Amount = productsInStock.Where(s => s.ProductId == x.Id)
                                 .Sum(s => s.Amount), 
                    Cost = x.Cost,
                    Shop = x.Shop,
                    Category = x.Category,
                    Code = x.Code,
                    BookedCount = bookingProductsAmount
                            .Where(z => z.ProductId == x.Id)
                            .Sum(z => z.Amount),
                    PrimeCost = productsInStock.Where(s => s.ProductId == x.Id)
                        .Sum(s => s.Amount * s.PrimeCost)
                }).ToList();

            return result;
        }

        public static ICollection<AllProductsVM> GetAllProductsBySupplier(ShopContext db, int supplierId)
        {
            var bookingProductsAmount = db.BookingProducts
                .Where(x => x.Booking.Status == BookingStatus.Open)
                .Select(x => new
                {
                    ProductId = x.ProductId,
                    Amount = x.Amount
                }).ToList();

            var productsInStock = db.SupplyProducts
                .Where(x => x.SupplierId == supplierId)
                .Select(x => new
                {
                    ProductId = x.ProductId,
                    Amount = x.StockAmount,
                    PrimeCost = x.ProcurementCost
                }).ToList();
            
            var result = db.Products
                .Include(x => x.Shop)
                .Include(x => x.Category)
                .OrderBy(x => x.Title)
                .ToList()
                .Select(x => new AllProductsVM()
                {
                    Id = x.Id,
                    Title = x.Title,
                    Amount = productsInStock.Where(s => s.ProductId == x.Id)
                                 .Sum(s => s.Amount) 
                             /*- bookingProductsAmount
                                 .Where(z => z.ProductId == x.Id)
                                 .Sum(z => z.Amount)*/,
                    Cost = x.Cost,
                    Shop = x.Shop,
                    Category = x.Category,
                    Code = x.Code,
                    BookedCount = bookingProductsAmount
                        .Where(z => z.ProductId == x.Id)
                        .Sum(z => z.Amount),
                    PrimeCost = productsInStock.Where(s => s.ProductId == x.Id)
                        .Sum(s => s.Amount * s.PrimeCost)
                }).ToList();

            return result;
        }

        public static ICollection<ProductInStockVM> GetProductsInStock(ShopContext db, PostgresContext postgresContext)
        {
            var bookedProducts = db.BookingProducts
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

            var incompleteProducts = postgresContext.IncompleteProducts
                .Select(x => new
                {
                    ProductId = x.ProductId,
                    Amount = x.Amount
                }).ToList()
                .GroupBy(x => x.ProductId)
                .Select(x => new
                {
                    ProductId = x.Key,
                    Amount = x.Sum(z => z.Amount)
                })
                .ToList();

            var productsInStock = db.SupplyProducts
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
                .Select(x => new ProductInStockVM
                {
                    Id = x.Key,
                    Amount = x.Sum(z => z.StockAmount)
                             - (bookedProducts.FirstOrDefault(z => z.ProductId == x.Key)?.Amount ?? 0),
                    Title = x.FirstOrDefault().Title,
                    Cost = x.FirstOrDefault().Cost,
                    Shop = x.FirstOrDefault().Shop,
                    Category = x.FirstOrDefault().Category,
                    Code = x.FirstOrDefault().Code,
                    BookedCount = bookedProducts
                        .FirstOrDefault(z => z.ProductId == x.Key)?.Amount ?? 0,
                    IncompleteCount = incompleteProducts
                        .FirstOrDefault(z => z.ProductId == x.Key)?.Amount ?? 0
                })
                .ToList();

            return productsInStock;
        }

        public static ICollection<AllProductsVM> GetAllProductsFilter(ShopContext db, ProductFilterVM filter)
        {
            var bookingProductsAmount = db.BookingProducts
                .Where(x => x.Booking.Status == BookingStatus.Open)
                .Where(x => filter.CategoryId == 0 || x.Product.Category.Id == filter.CategoryId)
                .Where(x => filter.ShopId == 0 || x.Product.Shop.Id == filter.ShopId)
                .Select(x => new
                {
                    ProductId = x.ProductId,
                    Amount = x.Amount
                }).ToList();

            var productsInStock = db.SupplyProducts
                .Where(x => filter.CategoryId == 0 || x.Product.Category.Id == filter.CategoryId)
                .Where(x => filter.ShopId == 0 || x.Product.Shop.Id == filter.ShopId)
                .Select(x => new
                {
                    ProductId = x.ProductId,
                    Amount = x.StockAmount
                }).ToList();

            var result = db.Products
                .Select(x => new AllProductsVM()
                {
                    Id = x.Id,
                    Title = x.Title,
                    Cost = x.Cost,
                    Shop = x.Shop,
                    Category = x.Category,
                    Code = x.Code,
                })
                .Where(x => filter.Title == null || x.Title.Contains(filter.Title))
                //.Where(x => productsInStock.Select(z => z.ProductId).Contains(x.Id)
                //    || bookingProductsAmount.Select(z => z.ProductId).Contains(x.Id))
                .Where(x => filter.CategoryId == 0 || x.Category.Id == filter.CategoryId)
                .Where(x => filter.ShopId == 0 || x.Shop.Id == filter.ShopId)
                .ToList()
                //.Where(x => productsInStock.Where(s => s.ProductId == x.Id)
                //                .Sum(s => s.Amount) > 0)
                .OrderBy(x => x.Title)
                .Select(x => new AllProductsVM()
                {
                    Id = x.Id,
                    Title = x.Title,
                    Amount = productsInStock.Where(s => s.ProductId == x.Id)
                                 .Sum(s => s.Amount) 
                             - bookingProductsAmount
                                 .Where(z => z.ProductId == x.Id)
                                 .Sum(z => z.Amount),
                    Cost = x.Cost,
                    Shop = x.Shop,
                    Category = x.Category,
                    Code = x.Code,
                    BookedCount = bookingProductsAmount
                        .Where(z => z.ProductId == x.Id)
                        .Sum(z => z.Amount)
                }).ToList();

            return result;
        }

        public static ICollection<ProductInStockVM> GetProductsInStockFilter(
            ShopContext db, PostgresContext postgresContext, ProductFilterVM filter)
        {
            var bookingProductsAmount = db.BookingProducts
                .Where(x => x.Booking.Status == BookingStatus.Open)
                .Where(x => filter.CategoryId == 0 || x.Product.Category.Id == filter.CategoryId)
                .Where(x => filter.ShopId == 0 || x.Product.Shop.Id == filter.ShopId)
                .Select(x => new
                {
                    ProductId = x.ProductId,
                    Amount = x.Amount
                }).ToList();
            
            var incompleteProducts = postgresContext.IncompleteProducts
                .Select(x => new
                {
                    ProductId = x.ProductId,
                    Amount = x.Amount
                }).ToList()
                .GroupBy(x => x.ProductId)
                .Select(x => new
                {
                    ProductId = x.Key,
                    Amount = x.Sum(z => z.Amount)
                })
                .ToList();

            var productsInStock = db.SupplyProducts
                .Where(x => x.StockAmount > 0)
                .Where(x => filter.CategoryId == 0 || x.Product.Category.Id == filter.CategoryId)
                .Where(x => filter.ShopId == 0 || x.Product.Shop.Id == filter.ShopId)
                .Select(x => new
                {
                    ProductId = x.ProductId,
                    Amount = x.StockAmount
                }).ToList();

            var result = db.Products
                .Select(x => new ProductInStockVM()
                {
                    Id = x.Id,
                    Title = x.Title,
                    Cost = x.Cost,
                    Shop = x.Shop,
                    Category = x.Category,
                    Code = x.Code,
                })
                .Where(x => filter.Title == null || x.Title.Contains(filter.Title))
                .Where(x => productsInStock.Select(z => z.ProductId).Contains(x.Id)
                    || bookingProductsAmount.Select(z => z.ProductId).Contains(x.Id))
                .Where(x => filter.CategoryId == 0 || x.Category.Id == filter.CategoryId)
                .Where(x => filter.ShopId == 0 || x.Shop.Id == filter.ShopId)
                .ToList()
                .Where(x => productsInStock.Where(s => s.ProductId == x.Id)
                                .Sum(s => s.Amount) > 0)
                .OrderBy(x => x.Title)
                .Select(x => new ProductInStockVM()
                {
                    Id = x.Id,
                    Title = x.Title,
                    Amount = productsInStock.Where(s => s.ProductId == x.Id)
                                 .Sum(s => s.Amount) 
                             - bookingProductsAmount
                                 .Where(z => z.ProductId == x.Id)
                                 .Sum(z => z.Amount),
                    Cost = x.Cost,
                    Shop = x.Shop,
                    Category = x.Category,
                    Code = x.Code,
                    BookedCount = bookingProductsAmount
                        .Where(z => z.ProductId == x.Id)
                        .Sum(z => z.Amount),
                    IncompleteCount = incompleteProducts
                        .Where(z => z.ProductId == x.Id)
                        .Sum(z => z.Amount)
                }).ToList();

            return result;
        }

        public static ProductDetailVM GetProductDetail(ShopContext db, int productId)
        {
            var productsInStock = db.SupplyProducts
                .Select(x => new
                {
                    ProductId = x.ProductId,
                    Amount = x.StockAmount
                }).ToList();
            
            var product = db.Products
                .Select(x => new Product()
                {
                    Id = x.Id,
                    Category = x.Category,
                    Code = x.Code,
                    Cost = x.Cost,
                    Title = x.Title,
                    Shop = x.Shop
                }).ToList()
                .Select(x => new ProductDetailVM()
                {
                    Id = x.Id,
                    Category = x.Category,
                    Code = x.Code,
                    Cost = x.Cost,
                    Title = x.Title,
                    Shop = x.Shop,
                    Amount = productsInStock.Where(s => s.ProductId == x.Id)
                        .Sum(s => s.Amount)
                })
                .First(p => p.Id == productId);

            return product;
        }
    }
}