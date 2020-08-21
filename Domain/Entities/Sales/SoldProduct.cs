using System;

namespace Domain.Entities.Sales
{
    public class SoldProduct
    {
        public Guid Id { get; private set; }

        public Guid SaleId { get; private set; }

        public Guid ProductId { get; private set; }

        public int Amount { get; private set; }

        public decimal Price { get; private set; }

        public SoldProduct(Guid id, Guid saleId, Guid productId, int amount, decimal price)
        {
            Id = id;
            SaleId = saleId;
            ProductId = productId;
            Amount = amount;
            Price = price;
        }
        
        private SoldProduct() {}
    }
}