namespace Domain.Entities.Sales
{
    public class SoldProduct : Entity
    {
        public int ProductId { get; private set; }
        public Product Product { get; private set; }

        public int SaleId { get; private set; }
        public Sale Sale { get; private set; }

        public int Amount { get; private set; }

        public SoldProduct(int productId, int saleId, int amount) =>
            (ProductId, SaleId, Amount) = (productId, saleId, amount);

        public SoldProduct(int productId, Sale sale, int amount) =>
            (ProductId, Sale, Amount) = (productId, sale, amount);
    }
}