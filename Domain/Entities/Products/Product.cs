namespace Domain.Entities.Products
{
    public class Product : Entity
    {
        public string Code { get; private set; }
        
        public string Title { get; private set; }

        public decimal Price { get; private set; }

        public int CategoryId { get; private set; }
        public ProductCategory Category { get; private set; }

        public Product(string title, int categoryId, string code, decimal price) =>
            (Title, CategoryId, Code, Price) = (title, categoryId, code, price);
    }

    public class ProductCategory : Entity
    {
        public string Title { get; private set; }

        public ProductCategory(string title) => Title = title;
    }
}