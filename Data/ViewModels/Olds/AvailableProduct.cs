namespace Data.ViewModels.Olds
{
    public class AvailableProduct
    {
        public int ProductId { get; set; }
        public int StockAmount { get; set; }
        public int ShopId { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
    }
}