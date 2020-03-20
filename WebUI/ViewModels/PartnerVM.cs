using Data.Entities;

namespace WebUI.ViewModels
{
    public class PartnerVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int BuyProductsAmount { get; set; }

        public static explicit operator Partner(PartnerVM objVM)
        {
            Partner obj = new Partner()
            {
                Id = objVM.Id,
                Title = objVM.Title,
                Phone = objVM.Phone,
                Email = objVM.Email
            };

            return obj;
        }
    }
}
