using System;
using Domain.Entities.Products;

namespace Domain.Commands
{
    public class SupplyProduct
    {
        public int ProductId { get; set; }
        public int ShopId { get; set; }
        public int SupplierId { get; set; }
        public int Amount { get; set; }
        public decimal ProcurementCost { get; set; }
        public SuppliedType Type { get; set; }
    }
}