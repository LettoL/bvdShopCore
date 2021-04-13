namespace Domain.Entities.Sales
{
    public class Manager : Entity
    {
        public string Name { get; set; }

        public Manager(string name) => Name = name;

        public Manager ChangeName(string name)
        {
            Name = name;

            return this;
        }
    }
}