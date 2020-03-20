using System.Collections.Generic;

namespace Data.ViewModels
{
    public class SalePaymentVM
    {
        public string Date { get; set; }
        public string SaleNumber { get; set; }
        public string MoneyWorker { get; set; }
        public string PaymentType { get; set; }
        public string Sum { get; set; }
        public decimal Discount { get; set; }
        public bool ForRF { get; set; }
        public int? SaleId { get; set; }
        public int? BookingId { get; set; }
        public int ShopId { get; set; }
        public string ShopTitle { get; set; }

        public string OperationType { get; set; }

        public IEnumerable<SaleProductItemVM> SaleProducts { get; set; }
    }

    public class SaleProductItemVM
    {
        public string Title { get; set; }
        public string Amount { get; set; }
    }
}
