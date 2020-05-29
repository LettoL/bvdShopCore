using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Handlers.Dtos.Products;
using Microsoft.EntityFrameworkCore;
using PostgresData;

namespace Handlers.QueryHandler
{
    public class GetProductHandler
    {
        public static async Task<ICollection<AllProductItem>> Handle(PostgresContext db)
        {
            var products = await db.SuppliedProducts
                .GroupBy(x => new
                {
                    x.ProductId,
                    x.ShopId,
                    x.Shop.Title
                })
                .Join(
                    db.Products,
                    supplied => supplied.Key.ProductId,
                    product => product.Id,
                    
                    (supplied, product) => new AllProductItem
                    {
                        ProductId = supplied.Key.ProductId,
                        ShopId = supplied.Key.ShopId,
                        Code = product.Code,
                        Title = product.Title,
                        Amount = supplied.Sum(p => p.StockAmount),
                        Price = product.Price,
                        ShopTitle = supplied.Key.Title,
                        CategoryId = product.CategoryId,
                        CategoryTitle = product.Category.Title
                    }).ToListAsync();
                /*.Select(x => new AllProductItem
                {
                    ProductId = x.Key.ProductId,
                    ShopId = x.Key.ShopId,
                    Code = x.First().Product.Code,
                    Title = x.First().Product.Title,
                    Amount = x.Sum(p => p.StockAmount),
                    Price = x.First().Product.Price,
                    ShopTitle = x.First().Shop.Title,
                    CategoryId = x.First().Product.CategoryId,
                    CategoryTitle = x.First().Product.Category.Title
                }).ToListAsync();*/

            return products;
        }
    }
}