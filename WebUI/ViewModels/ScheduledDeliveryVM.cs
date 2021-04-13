namespace WebUI.ViewModels
{
    public class ScheduledDeliveryVM
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public int SupplierId { get; set; }
        public string Supplier { get; set; }
        public decimal Payment { get; set; }
        public decimal ProcurementCost { get; set; }
        public int ProductsExpected { get; set; }
        public string ShopsTitles { get; set; }
        public string MoneyWorkersTitles { get; set; }
    }
}