using System.Collections.Generic;

namespace WebUI.ViewModels
{
    public class CreateScheduledDeliveryVM
    {
        public int SupplierId { get; set; }
        public decimal DepositedSum { get; set; }
        public int CategoryId { get; set; }
        public ICollection<CreateScheduledProductDeliveryVM> Products { get; set; }
        public int MoneyWorkerId { get; set; }
        public int ShopId { get; set; }
    }

    public class CreateScheduledProductDeliveryVM
    {
        public int ProductId { get; set; }
        public int Amount { get; set; }
        public decimal ProcurementCost { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
    }
}