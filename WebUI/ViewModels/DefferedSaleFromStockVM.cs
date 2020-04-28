using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.ViewModels;

namespace WebUI.ViewModels
{
    public class DefferedSaleFromStockVM
    {
        public ICollection<ProductSaleCreateVM> Products { get; set; }
        public decimal Sum { get; set; }
        public decimal CashSum { get; set; }
        public decimal CashlessSum { get; set; }
        public decimal Discount { get; set; }
        public string Buyer { get; set; }
        public string Comment { get; set; }
        public int UserId { get; set; }
        public int? MoneyWorkerId { get; set; }
        public int? MoneyWorkerIdForIncome { get; set; }
        public int? MoneyWorkerIdForExpense { get; set; }
        public bool ForRussian { get; set; }
        public string Manager { get; set; }
    }
}
