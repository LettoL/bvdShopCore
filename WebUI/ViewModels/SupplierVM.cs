using Data.Entities;

namespace WebUI.ViewModels
{
    public class SupplierVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public decimal Debt { get; set; }
        public decimal CostRealizationProductOnStock { get; set; }
        public decimal CostProductOnStock { get; set; }

        public static explicit operator Supplier(SupplierVM objVM)
        {
            Supplier obj = new Supplier()
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
