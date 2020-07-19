namespace Domain.Entities
{
    public class IncompleteProduct : Entity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Amount { get; set; }

        public string Comment { get; set; }

        public int ShopId { get; set; }
        public Shop Shop { get; set; }

        public IncompleteProduct(int productId, int amount, int shopId, string comment = "")
            => (ProductId, Amount, ShopId, Comment) = (productId, amount, shopId, comment);
    }
}