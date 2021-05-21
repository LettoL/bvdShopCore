using System.Collections.Generic;

namespace WebUI.ViewModels
{
    public class SaledProductsVM
    {
        public int ProductId { get; set; }
        public int Amount { get; set; }
        public int CategoryId { get; set; }
        public int ShopId { get; set; }
        public ICollection<int> SuppliersId { get; set; } = new HashSet<int>();
        public string Title { get; set; }
        public string ShopTitle { get; set; }
    }
}