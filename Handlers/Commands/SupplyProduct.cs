using System;
using Domain.Entities;

namespace Handlers.Commands
{
    public class SupplyProduct
    {
        public int ProductId { get; set; }
        public int ShopId { get; set; }
        public int SupplierId { get; set; }
        public int Amount { get; set; }
        public decimal ProcurementCost { get; set; }
        public SuppliedType Type { get; set; }

        public SuppliedProduct CreateSuppliedProduct(DateTime currentDate)
        {
            return new SuppliedProduct(
                currentDate, Amount, ProcurementCost, ShopId, ProductId, Type, SupplierId);
        }
    }
}