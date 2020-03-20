using Data.Entities;
using Data.Enums;

namespace WebUI.ViewModels
{
    public class UserVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Password { get; set; }
        public Role Role{ get; set; }
        public Shop Shop { get; set; }
    }
}
