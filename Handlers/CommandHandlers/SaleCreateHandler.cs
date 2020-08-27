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
        /*public static async Task<Sale> Execute(this SaleCreate command, PostgresContext db)
        {
            var saleId = Guid.NewGuid();

            var suppliedProductsAvailableForSale = GetSuppliedProductsAvailableForSale.Execute(
                command.SellingProducts
                    .Select(x => x.ProductId).ToList(),
                command.ShopId,
                db);
            
            var soldProducts = SoldProducts(
                saleId, command.SellingProducts, suppliedProductsAvailableForSale.Result);
            //deposited moneys
            //sale
        }*/

        private static ICollection<SoldProduct> SoldProducts (
            Guid saleId,
            ICollection<SellingProduct> sellingProducts,
            ICollection<SuppliedProductAvailableForSale> suppliedProducts)
        {
            var soldProducts = new HashSet<SoldProduct>();

            foreach (var sellingProduct in sellingProducts)
            {
                var soldProductId = Guid.NewGuid();

                var soldFromSupplies =
                    SoldProduct(sellingProduct.ProductId, soldProductId, sellingProduct.Amount, suppliedProducts);

                soldProducts.Add(new SoldProduct(
                    soldProductId, saleId, sellingProduct.ProductId, sellingProduct.Amount,
                    sellingProduct.Price, soldFromSupplies.Item1));

                suppliedProducts = soldFromSupplies.Item2;
            }

            var result = soldProducts;

            return result;
        }

        private static (ICollection<SoldFromSupply>, ICollection<SuppliedProductAvailableForSale>) SoldProduct(
            Guid productId, Guid soldProductId, int amount, ICollection<SuppliedProductAvailableForSale> suppliedProducts)
        {
            var soldFromSupplies = new List<SoldFromSupply>();

            if (AvailableSoldForRealization(productId, suppliedProducts))
            {
                //Получаем поставку под реализацию
                var suppliedForSold = suppliedProducts
                    .Where(x => x.ProductId == productId
                                && x.ForRealization)
                    .OrderBy(x => x.AvailableAmount)
                    .FirstOrDefault();

                if (amount <= suppliedForSold.AvailableAmount)
                {
                    var soldFromSupply = new SoldFromSupply(
                        Guid.NewGuid(), soldProductId, suppliedForSold.SuppliedProductId,
                        suppliedForSold.ProcurementCost, amount);
                    
                    //вычесть проданные товары из доступных для продажи

                    soldFromSupplies.Add(soldFromSupply);

                    return (soldFromSupplies, suppliedProducts);
                }
                else
                {
                    var soldFromSupply = new SoldFromSupply(
                        Guid.NewGuid(), soldProductId, suppliedForSold.SuppliedProductId,
                        suppliedForSold.ProcurementCost, suppliedForSold.AvailableAmount);
                    
                    soldFromSupplies.Add(soldFromSupply);

                    amount -= soldFromSupply.Amount;

                    var result = 
                        SoldProduct(productId, soldProductId, amount, suppliedProducts);

                    soldFromSupplies = soldFromSupplies.Concat(result.Item1).ToList();

                    return (soldFromSupplies, result.Item2);
                }
            }
            else
            {
                var suppliedForSold = suppliedProducts
                    .Where(x => x.ProductId == productId)
                    .OrderBy(x => x.AvailableAmount)
                    .FirstOrDefault();
                
                if (amount <= suppliedForSold.AvailableAmount)
                {
                    var soldFromSupply = new SoldFromSupply(
                        Guid.NewGuid(), soldProductId, suppliedForSold.SuppliedProductId,
                        suppliedForSold.ProcurementCost, amount);
                    
                    //вычесть проданные товары из доступных для продажи

                    soldFromSupplies.Add(soldFromSupply);

                    return (soldFromSupplies, suppliedProducts);
                }
                else
                {
                    var soldFromSupply = new SoldFromSupply(
                        Guid.NewGuid(), soldProductId, suppliedForSold.SuppliedProductId,
                        suppliedForSold.ProcurementCost, suppliedForSold.AvailableAmount);
                    
                    soldFromSupplies.Add(soldFromSupply);

                    amount -= soldFromSupply.Amount;

                    var result = 
                        SoldProduct(productId, soldProductId, amount, suppliedProducts);

                    soldFromSupplies = soldFromSupplies.Concat(result.Item1).ToList();

                    return (soldFromSupplies, result.Item2);
                }
            }
        }

        private static bool AvailableSoldForRealization(
            Guid productId, ICollection<SuppliedProductAvailableForSale> suppliedProductAvailableForSales)
        {
            var result = suppliedProductAvailableForSales
                .Any(x => x.ProductId == productId
                          && x.ForRealization && x.AvailableAmount > 0);

            return result;
        }
    }
}