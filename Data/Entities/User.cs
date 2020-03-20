using Base;
using Data.Enums;

namespace Data.Entities
{
    public class User : BaseObject
    {
        public string Login { get; set; }

        public string Password { get; set; }

        public Role Role { get; set; }

        public int? ShopId { get; set; }
        public Shop Shop { get; set; }
    }
}
