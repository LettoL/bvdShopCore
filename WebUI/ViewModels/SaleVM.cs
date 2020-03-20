using Data.Entities;
using Data.Enums;

namespace WebUI.ViewModels
{
    public class SaleVM
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public decimal Sum { get; set; }
        public string ShopTitle { get; set; }
        public string ProductTitle { get; set; }
        public PaymentType PaymentType { get; set; }
        public SaleType SaleType { get; set; }
        public bool HasAdditionalProduct { get; set; }

        public string Comment { get; set; }
        public string BuyerTitle { get; set; }

        public decimal PrimeCost { get; set; }
        public decimal Rest { get; set; }
        public decimal Total { get; set; }
    }
}
