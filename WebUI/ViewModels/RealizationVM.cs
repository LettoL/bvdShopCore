using System.Collections.Generic;
using Data.ViewModels;

namespace WebUI.ViewModels
{
    public class RealizationVM
    {
        public int UserId { get; set; }

        public ICollection<ProductSaleCreateVM> Products { get; set; }
        public decimal Sum { get; set; }
        public decimal CashSum { get; set; }
        public decimal CashlessSum { get; set; }
        public decimal Discount { get; set; }
        public bool Cashless { get; set; }
        public string Buyer { get; set; }
        public string AdditionalComment { get; set; }
        public string Comment { get; set; }
        public int? MoneyWorkerId { get; set; }
        public bool Payment { get; set; }
        public bool ForRussian { get; set; }
        public string Manager { get; set; }
    }
}