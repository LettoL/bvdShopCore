using Base;

namespace Data.Entities
{
    public class SupplyProduct : BaseObject
    {
        public int? SupplierId { get; set; }
        public Supplier Supplier { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int? SupplyHistoryId { get; set; }
        public SupplyHistory SupplyHistory { get; set; }

        public int TotalAmount { get; set; }

        public int RealizationAmount { get; set; }

        public int StockAmount { get; set; }       

        public decimal AdditionalCost { get; set; }

        public decimal ProcurementCost { get; set; }

        public decimal FinalCost { get; set; }
    }
}