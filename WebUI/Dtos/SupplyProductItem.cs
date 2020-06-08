using System;

namespace WebUI.Dtos
{
    public class SupplyProductItem
    {
        public int SupplyProductId { get; set; }
        public DateTime DateTime { get; set; }
        public int ProductId { get; set; }
        public string ProductTitle { get; set; }
        public int ShopAmount { get; set; }
        public int SuppliedAmount { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public int ShopId { get; set; }
        public string ShopTitle { get; set; }
        public decimal ProcurementCost { get; set; }
    }
}