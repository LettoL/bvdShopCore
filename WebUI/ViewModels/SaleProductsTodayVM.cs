using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.ViewModels
{
    public class SaleProductsTodayVM
    {
        public string ShopTitle { get; set; }
        public ICollection<SaleProduct> SalesProducts { get; set; }
        public decimal Sum { get; set; }
    }
}
