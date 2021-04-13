using Data.Entities;

namespace Data.ViewModels
{
    public class AllProductsVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public int Amount { get; set; }
        public int BookedCount { get; set; }
        public decimal Cost { get; set; }
        public Shop Shop { get; set; }
        public Category Category { get; set; }
        public decimal PrimeCost { get; set; }
    }
}