namespace Application.Entities
{
    public class Supplier : Entity
    {
        public string Name { get; private set; }

        public Supplier(string name) => Name = name;
    }
}