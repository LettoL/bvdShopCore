namespace Handlers.Dtos.Products
{
    public class AllProductItem
    {
        public int ProductId { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public int ShopId { get; set; }
        public string ShopTitle { get; set; }
        public int CategoryId { get; set; }
        public string CategoryTitle { get; set; }
    }
}