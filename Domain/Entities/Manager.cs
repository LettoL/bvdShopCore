namespace Domain.Entities
{
    public class Manager : Entity
    {
        public string Name { get; private set; }

        public Manager(string name) => Name = name;
    }
}