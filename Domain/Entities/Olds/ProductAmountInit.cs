namespace Domain.Entities.Olds
{
    public class ProductAmountInit : Entity
    {
        public int ProductId { get; private set; }
        public int Amount { get; private set; }
        public decimal Price { get; private set; }

        public ProductAmountInit(int productId, int amount, decimal price) =>
            (ProductId, Amount, Price) = (productId, amount, price);
    }
}