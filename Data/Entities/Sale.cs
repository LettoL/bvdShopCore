using System;
using System.Collections.Generic;
using Base;
using Data.Enums;

namespace Data.Entities
{
    public class Sale : BaseObject
    {
        public string Title { get; set; }

        public string AdditionalComment { get; set; }
        public string Comment { get; set; }

        public DateTime Date { get; set; }

        public decimal Sum { get; set; }
        public decimal CashSum { get; set; }
        public decimal CashlessSum { get; set; }

        public decimal PrimeCost { get; set; }

        public decimal Margin { get; set; }

        public decimal Discount { get; set; }

        public bool Payment { get; set; }
        public bool ForRussian { get; set; }

        public int ShopId { get; set; }
        public Shop Shop { get; set; }

        public int? PartnerId { get; set; }
        public Partner Partner { get; set; }
        
        public SaleType SaleType { get; set; }

        public ICollection<SaleProduct> SalesProducts { get; set; }
    }
}
