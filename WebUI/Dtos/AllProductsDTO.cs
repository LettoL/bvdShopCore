namespace WebUI.Dtos
{
    public class AllProductsDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public int Amount { get; set; }
        public int BookedCount { get; set; }
        public decimal Cost { get; set; }
        public int ShopId { get; set; }
        public string ShopTitle { get; set; }
        public int CategoryId { get; set; }
        public string CategoryTitle { get; set; }
        public decimal PrimeCost { get; set; }
    }
}