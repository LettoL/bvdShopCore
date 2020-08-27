using System;
using System.Collections.Generic;

namespace Domain.Entities.Sales
{
    public class SoldProduct
    {
        public Guid Id { get; private set; }

        public Guid SaleId { get; private set; }

        public Guid ProductId { get; private set; }

        public int Amount { get; private set; }

        public decimal Price { get; private set; }
        
        public ICollection<SoldFromSupply> SoldFromSupplies { get; private set; } = new HashSet<SoldFromSupply>();

        public SoldProduct(Guid id, Guid saleId, Guid productId, int amount, decimal price,
            ICollection<SoldFromSupply> soldFromSupplies)
        {
            Id = id;
            SaleId = saleId;
            ProductId = productId;
            Amount = amount;
            Price = price;
            SoldFromSupplies = soldFromSupplies;
        }
        
        private SoldProduct() {}
    }
}