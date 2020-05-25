namespace Domain.Entities
{
    public class User : Entity
    {
        public string Login { get; private set; }
        public string Password { get; private set; }
        public UserRole Role { get; private set; }

        public User(string login, string password, UserRole role) =>
            (Login, Password, Role) = (login, password, role);
    }

    public enum UserRole
    {
        Admin = 10,
        
        Manager = 20
    }
}