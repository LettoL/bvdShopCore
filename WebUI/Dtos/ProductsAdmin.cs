namespace WebUI.Dtos
{
    public class ProductsAdmin
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public string Shop { get; set; }
        public int ShopId { get; set; }
        public string Category { get; set; }
        public int CategoryId { get; set; }
        public decimal PrimeCost { get; set; }
    }
}