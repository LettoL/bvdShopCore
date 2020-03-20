using System.Collections.Generic;
using Base;

namespace Data.Entities
{
    public class Product : BaseObject
    {
        public string Title { get; set; }

        public string Code { get; set; }

        public bool Reserv { get; set; }

        public int BookingAmount { get; set; }

        public decimal Cost { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int ShopId { get; set; }
        public Shop Shop { get; set; }

        public ICollection<PartnerProduct> PartnersProducts { get; set; }

        public ICollection<SaleProduct> SalesProducts { get; set; }

        public ICollection<SupplyProduct> SupplierProducts { get; set; }
    }
}
