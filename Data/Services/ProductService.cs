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
        public static ICollection<AllProductsVM> GetAllProducts(ShopContext db)
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
                    Amount = x.StockAmount
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

        public static ICollection<ProductInStockVM> GetProductsInStockFilter(ShopContext db, ProductFilterVM filter)
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