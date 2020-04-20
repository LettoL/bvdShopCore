using System.Collections.Generic;
using System.Linq;
using Data.Entities;
using Data.Enums;
using Data.ViewModels;
using Microsoft.EntityFrameworkCore;

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