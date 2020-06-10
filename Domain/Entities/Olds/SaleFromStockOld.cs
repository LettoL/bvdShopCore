using System.Collections.Generic;

namespace Domain.Entities.Olds
{
    public class SaleFromStockOld : Entity
    {
        public int SaleId { get; set; }
        public int SupplierId { get; set; }

        public ICollection<SoldProductFromStockOld> Products { get; set; } = new HashSet<SoldProductFromStockOld>();
    }

    public class SoldProductFromStockOld : Entity
    {
        public int ProductId { get; set; }
        public int SaleFromStockOldId { get; set; }
        public SaleFromStockOld SaleFromStockOld { get; set; }
        public decimal ProcurementCost { get; set; }
    }
}