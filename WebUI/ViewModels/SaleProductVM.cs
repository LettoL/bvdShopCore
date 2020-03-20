using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.ViewModels
{
    public class SaleProductVM
    {
        public string Title { get; set; }
        public string Code { get; set; }
        public string Date { get; set; }
        public string AdditionalComment { get; set; }
        public int Amount { get; set; }
        public decimal Cost { get; set; }
        public string ShopTitle { get; set; }
        public int SaleNumber { get; set; }
    }
}
