using Base;

namespace Data.Entities
{
    public class SaleProduct : BaseObject
    {
        public int SaleId { get; set; }
        public Sale Sale { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Amount { get; set; }

        public bool Additional { get; set; }

        public decimal Cost { get; set; }
    }
}