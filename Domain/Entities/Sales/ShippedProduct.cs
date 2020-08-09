using Domain.Entities.Products;

namespace Domain.Entities.Sales
{
    public class ShippedProduct : Entity
    {
        public int ProductId { get; private set; }
        public Product Product { get; private set; }

        public int SoldProductId { get; private set; }
        public SoldProduct SoldProduct { get; private set; }

        public int Amount { get; private set; }

        public ShippedProduct(int productId, int soldProductId, int amount) =>
            (ProductId, SoldProductId, Amount) = (productId, soldProductId, amount);
    }
}