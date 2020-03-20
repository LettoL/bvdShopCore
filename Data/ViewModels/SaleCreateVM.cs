using System.Collections.Generic;
using Data.Enums;

namespace Data.ViewModels
{
    public class SaleCreateVM
    {
        public int? PartnerId { get; set; }
        public int UserId { get; set; }
        public int? MoneyWorkerId { get; set; }
        public string AdditionalComment { get; set; }
        public string Comment { get; set; }
        public decimal Sum { get; set; }
        public decimal CashSum { get; set; }
        public decimal CashlessSum { get; set; }
        public decimal Discount { get; set; }
        public bool Payment { get; set; }
        public bool ForRussian { get; set; }
        public PaymentType PaymentType { get; set; }
        public SaleType SaleType { get; set; }
        public ICollection<ProductSaleCreateVM> Products { get; set; }
    }
}
