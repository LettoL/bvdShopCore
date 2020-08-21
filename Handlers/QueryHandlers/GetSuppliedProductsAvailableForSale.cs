using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities.Supplies;
using Microsoft.EntityFrameworkCore;
using PostgresData;

namespace Handlers.QueryHandlers
{
    public static class GetSuppliedProductsAvailableForSale
    {
        public static async Task<ICollection<SuppliedProductAvailableForSale>> Execute
            (ICollection<Guid> productsId, Guid shopId, PostgresContext db)
        {
            var suppliedProducts = await db.SuppliedProducts
                .Where(x => productsId.Contains(x.ProductId) && x.ShopId == shopId)
                .ToListAsync();

            var suppliedProductsId = suppliedProducts
                .Select(x => x.Id)
                .ToList();

            var soldProducts = await db.SoldFromSupplies
                .Where(x => suppliedProductsId.Contains(x.SuppliedProductId))
                .ToListAsync();

            var result = suppliedProducts
                .Select(x => new SuppliedProductAvailableForSale
                {
                    SuppliedProductId = x.Id,
                    AvailableAmount = x.Amount - soldProducts
                        .Where(z => z.SuppliedProductId == x.Id)
                        .Sum(z => z.Amount),
                    ForRealization = x.Type == SupplyType.ForRealization ? true : false,
                    ProductId = x.ProductId,
                    ProcurementCost = x.ProcurementCost
                })
                .Where(x => x.AvailableAmount > 0)
                .ToList();

            return result;
        }
    }

    public class SuppliedProductAvailableForSale
    {
        public Guid SuppliedProductId { get; set; }
        public int AvailableAmount { get; set; }
        public bool ForRealization { get; set; }
        public Guid ProductId { get; set; }
        public decimal ProcurementCost { get; set; }
    }
}