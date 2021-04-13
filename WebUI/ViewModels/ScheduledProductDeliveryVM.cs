namespace WebUI.ViewModels
{
    public class ScheduledProductDeliveryVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Supplier { get; set; }
        public string Shop { get; set; }
        public int Amount { get; set; }
        public decimal ProcurementCost { get; set; }
        public int ShopId { get; set; }
        public int SupplierId { get; set; }
        public int ProductId { get; set; }
        public int SupplyProductId { get; set; }
    }
}