using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.ViewModels;

namespace WebUI.ViewModels
{
    public class SaleFromStockVM
    {
        public ICollection<ProductSaleCreateVM> Products { get; set; }
        public decimal Sum { get; set; }
        public decimal Cash { get; set; }
        public decimal Cashless { get; set; }
        public decimal Discount { get; set; }
        public string Buyer { get; set; }
        public string AdditionalComment { get; set; }
        public string Comment { get; set; }
        public int UserId { get; set; }
        public int? MoneyWorkerIdForCashlessIncome { get; set; }
        public bool ForRussian { get; set; }
    }
}
