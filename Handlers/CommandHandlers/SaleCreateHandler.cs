using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Commands.Sales;
using Domain.Entities.Sales;
using Handlers.QueryHandlers;
using Microsoft.EntityFrameworkCore.Internal;
using PostgresData;

namespace Handlers.CommandHandlers
{
    public static class SaleCreateHandler
    {
       /* public static async Task<Sale> Execute(this SaleCreate command, PostgresContext db)
        {
            var saleId = Guid.NewGuid();

            var suppliedProductsAvailableForSale = GetSuppliedProductsAvailableForSale.Execute(
                command.SellingProducts
                    .Select(x => x.ProductId).ToList(),
                command.ShopId,
                db);
            
            
        }*/

        private static (ICollection<SoldProduct>, ICollection<SoldFromSupply>) SoldProducts (
            ICollection<SellingProduct> sellingProducts,
            ICollection<SuppliedProductAvailableForSale> suppliedProducts)
        {
            var soldProducts = new HashSet<SoldProduct>();
            var soldFromSupplies = new HashSet<SoldFromSupply>();

            foreach (var sellingProduct in sellingProducts)
            {
                //var sold =
            }

            var result = (soldProducts, soldFromSupplies);

            return result;
        }

       /* private static (SoldProduct, ICollection<SoldFromSupply>, ICollection<SuppliedProductAvailableForSale>) SoldProduct(
            Guid saleId, SellingProduct sellingProduct, ICollection<SuppliedProductAvailableForSale> suppliedProducts)
        {
            if (AvailableSoldForRealization(sellingProduct.ProductId, suppliedProducts))
            {
                var suppliedForSold = suppliedProducts
                    .Where(x => x.ProductId == sellingProduct.ProductId
                                && x.ForRealization)
                    .OrderBy(x => x.AvailableAmount)
                    .FirstOrDefault();
                
                if (sellingProduct.Amount <= suppliedForSold.AvailableAmount)
                {
                    var soldProduct = new SoldProduct(
                        Guid.NewGuid(), saleId, sellingProduct.ProductId, sellingProduct.Amount, sellingProduct.Price);

                    suppliedForSold.AvailableAmount -= soldProduct.Amount;//проверить изменится ли значение в коллекции
                    sellingProduct.Amount -= soldProduct.Amount;
                    
                    if(sellingProduct.Amount != 0)
                        throw new Exception("");
                }
                else
                {
                    var soldProduct = new SoldProduct(
                        Guid.NewGuid(), saleId, sellingProduct.ProductId, suppliedForSold.AvailableAmount, sellingProduct.Price);

                    suppliedForSold.AvailableAmount -= soldProduct.Amount;
                    sellingProduct.Amount -= soldProduct.Amount;
                    
                    if(suppliedForSold.AvailableAmount != 0)
                        throw new Exception("");

                    var test = SoldProduct(saleId, sellingProduct, suppliedProducts);
                }
            }
            else
            {
                var suppliedForSold = suppliedProducts
                    .Where(x => x.ProductId == sellingProduct.ProductId)
                    .OrderBy(x => x.AvailableAmount)
                    .FirstOrDefault();
                
                if (sellingProduct.Amount <= suppliedForSold.AvailableAmount)
                {
                    var soldProduct = new SoldProduct(
                        Guid.NewGuid(), saleId, sellingProduct.ProductId, sellingProduct.Amount, sellingProduct.Price);

                    suppliedForSold.AvailableAmount -= soldProduct.Amount;//проверить изменится ли значение в коллекции
                    sellingProduct.Amount -= soldProduct.Amount;
                    
                    if(sellingProduct.Amount != 0)
                        throw new Exception("");
                }
                else
                {
                    var soldProduct = new SoldProduct(
                        Guid.NewGuid(), saleId, sellingProduct.ProductId, suppliedForSold.AvailableAmount, sellingProduct.Price);

                    suppliedForSold.AvailableAmount -= soldProduct.Amount;
                    sellingProduct.Amount -= soldProduct.Amount;
                    
                    if(suppliedForSold.AvailableAmount != 0)
                        throw new Exception("");

                    var test = SoldProduct(saleId, sellingProduct, suppliedProducts);
                }
            }
        }*/

        private static bool AvailableSoldForRealization(
            Guid productId, ICollection<SuppliedProductAvailableForSale> suppliedProductAvailableForSales)
        {
            var result = suppliedProductAvailableForSales
                .Any(x => x.ProductId == productId
                          && x.ForRealization && x.AvailableAmount > 0);

            return result;
        }

        private static void SoldForRealization(
            Guid saleId, SellingProduct sellingProduct, ICollection<SuppliedProductAvailableForSale> suppliedProducts)
        {
            var suppliedForSold = suppliedProducts
                .Where(x => x.ProductId == sellingProduct.ProductId
                            && x.ForRealization)
                .OrderBy(x => x.AvailableAmount)
                .FirstOrDefault();

            if (sellingProduct.Amount <= suppliedForSold.AvailableAmount)
            {
                var soldProduct = new SoldProduct(
                    Guid.NewGuid(), saleId, sellingProduct.ProductId, sellingProduct.Amount, sellingProduct.Price);

                suppliedForSold.AvailableAmount -= soldProduct.Amount;//проверить изменится ли значение в коллекции
                sellingProduct.Amount -= soldProduct.Amount;
            }

        }

        private static void Sold(
            SellingProduct sellingProduct, ICollection<SuppliedProductAvailableForSale> suppliedProducts)
        {
            var suppliedForSold = suppliedProducts
                .Where(x => x.ProductId == sellingProduct.ProductId)
                .OrderBy(x => x.AvailableAmount)
                .FirstOrDefault();
        }
    }
}