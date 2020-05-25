namespace Application.Entities
{
    public class Product : Entity
    {
        public string Title { get; private set; }

        public int CategoryId { get; private set; }
        public ProductCategory Category { get; private set; }

        public Product(string title, int categoryId) =>
            (Title, CategoryId) = (title, categoryId);
    }

    public class ProductCategory : Entity
    {
        public string Title { get; private set; }

        public ProductCategory(string title) => Title = title;
    }
}