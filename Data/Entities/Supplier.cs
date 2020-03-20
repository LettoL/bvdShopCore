using System.Collections.Generic;
using Base;

namespace Data.Entities
{
    public class Supplier : BaseObject
    {
        public string Title { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public decimal Debt { get; set; }

        public ICollection<SupplyProduct> SupplyProducts { get; set; }
    }
}