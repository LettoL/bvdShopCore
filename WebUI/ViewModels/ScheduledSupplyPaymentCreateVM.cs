namespace WebUI.ViewModels
{
    public class ScheduledSupplyPaymentCreateVM
    {
        public int SupplyId { get; set; }
        public decimal Sum { get; set; }
        public int MoneyWorkerId { get; set; }
        public int ShopId { get; set; }
    }
}