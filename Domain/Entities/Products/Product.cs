using System;

namespace Domain.Entities.Products
{
    public class Product
    {
        public Guid Id { get; private set; }
        
        public string Code { get; private set; }
        
        public string Title { get; private set; }

        public decimal Price { get; private set; }

        public int CategoryId { get; private set; }
        public ProductCategory Category { get; private set; }

        public Product(Guid id, string title, int categoryId, string code, decimal price) =>
            (Id, Title, CategoryId, Code, Price) = (id, title, categoryId, code, price);
    }

    public class ProductCategory
    {
        public Guid Id { get; private set; }
        
        public string Title { get; private set; }

        public ProductCategory(Guid id, string title) => (Id, Title) = (id, title);
    }
}