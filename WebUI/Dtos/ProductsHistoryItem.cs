using System;

namespace WebUI.Dtos
{
    public class ProductsHistoryItem
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string ProductTitle { get; set; }
        public int Amount { get; set; }
        public string SupplierName { get; set; }
        public string Type { get; set; }
        public string ShopTitle { get; set; }
        public int ShopId { get; set; }
    }
}