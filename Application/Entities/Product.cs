namespace Application.Entities
{
    public class Product : Entity
    {
        public string Title { get; private set; }

        public int CategoryId { get; private set; }
        public ProductCategory Category { get; private set; }
    }

    public class ProductCategory : Entity
    {
        public string Title { get; private set; }
    }
}