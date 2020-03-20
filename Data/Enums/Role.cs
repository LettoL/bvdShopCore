using System.ComponentModel;

namespace Data.Enums
{
    public enum Role
    {
        [Description("Администратор")]
        Administrator = 1,

        [Description("Менеджер")]
        Manager = 2
    }
}
