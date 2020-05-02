using System;
using System.Collections;
using System.Collections.Generic;

namespace WebUI.Commands
{
    public class ImportProducts
    {
        public ICollection<ProductForImport> Products { get; set; }
        public int ShopId { get; set; }
        public int SupplierId { get; set; }
        public int CategoryId { get; set; }
        public decimal additionalCost { get; set; }
        public int Realization { get; set; }
        public DateTime? Date { get; set; }
    }

    public class ProductForImport
    {
        public int Number { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public decimal Sum { get; set; }
    }
}