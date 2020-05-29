using System.Collections.Generic;

namespace WebUI.Commands
{
    public class ImportProductsForm
    {
        public string ProductsText { get; set; }
        public int Shop { get; set; }
        public int Supplier { get; set; }
        public int SupplyType { get; set; }
        public int Category { get; set; }
        public ICollection<SupplyProductItem> Products { get; set; } = new HashSet<SupplyProductItem>();
    }

    public class SupplyProductItem
    {
        public int Number { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public int Amount { get; set; }
        public string Price { get; set; }
        //public int Sum { get; set; }
    }
}