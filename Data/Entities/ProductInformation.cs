using Base;

namespace Data.Entities
{
    public class ProductInformation : BaseObject
    {
        public int? SupplyProductId { get; set; }
        public SupplyProduct SupplyProduct { get; set; }

        public int SaleId { get; set; }
        public Sale Sale { get; set; }

        public int? ProductId { get; set; }
        public Product Product { get; set; }

        public decimal FinalCost { get; set; }
        public decimal AdditionalCost { get; set; }
        public decimal ProcurementCost { get; set; }
    
        public int Amount { get; set; }

        public bool ForRealization { get; set; }
    }
}
