namespace Domain.Entities.Supplies
{
    public class Supplier : Entity
    {
        public string Name { get; private set; }

        public string Phone { get; private set; }

        public string Email { get; private set; }

        public Supplier(string name, string phone, string email) =>
            (Name, Phone, Email) = (name, phone, email);
    }
}