using Domain.Commands;
using Domain.Entities.Products;
using Domain.Entities.Supplies;

namespace Domain.Entities.Sales
{
    public class SoldProduct : Entity
    {
        public int ProductId { get; private set; }
        public Product Product { get; private set; }

        public int SaleId { get; private set; }
        public Sale Sale { get; private set; }

        public int Amount { get; private set; }

        public int SuppliedProductId { get; private set; }
        public SuppliedProduct SuppliedProduct { get; private set; }

        public decimal Price { get; private set; }

        public SoldProduct(int productId, int saleId, int amount, int suppliedProductId, decimal price) =>
            (ProductId, SaleId, Amount, SuppliedProductId, Price) = 
            (productId, saleId, amount, suppliedProductId, price);

        public SoldProduct(int productId, Sale sale, int amount) =>
            (ProductId, Sale, Amount) = (productId, sale, amount);

        public static SoldProduct Create(SoldProductCreate command, Sale sale)
        {
            return new SoldProduct(command.ProductId, sale, command.Amount);
        }
    }
}