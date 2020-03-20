using System;
using System.Collections.Generic;
using System.Text;
using Base;

namespace Data.Entities
{
    public class DeferredSupplyProduct : BaseObject
    {
        public DateTime? Date { get; set; }

        public int SupplyProductId { get; set; }
        public SupplyProduct SupplyProduct { get; set; }
    }
}
