using System;
using Base;
using Data.Enums;

namespace Data.Entities
{
    public class InfoProduct : BaseObject
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int ShopId { get; set; }
        public Shop Shop { get; set; }

        public int? SupplierId { get; set; }
        public Supplier Supplier { get; set; }

        public int? SaleId { get; set; }
        public Sale Sale { get; set; }

        public int? SupplyHistoryId { get; set; }
        public SupplyHistory SupplyHistory { get; set; }

        public int? SupplyProductId { get; set; }
        public SupplyProduct SupplyProduct { get; set; }

        public int Amount { get; set; }

        public DateTime Date { get; set; }

        public InfoProductType Type { get; set; }
    }
}
