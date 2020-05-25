namespace Application.Entities
{
    public class Shop : Entity
    {
        public string Title { get; private set; }

        public Shop(string title) => Title = title;
    }
}