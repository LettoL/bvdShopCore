namespace WebUI.ViewModels
{
    public class ShopVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal CashOnHand { get; set; }
        public decimal Turnover { get; set; }
        public int Sales { get; set; }
        public decimal Margin { get; set; }
    }
}
