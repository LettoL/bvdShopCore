using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.ViewModels
{
    public class ProductSaleCreateVM
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public bool Additional { get; set; }
        public decimal Cost { get; set; }
        public decimal ProcurementCost { get; set; }
    }
}
