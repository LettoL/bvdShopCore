using System.Collections.Generic;

namespace WebUI.ViewModels
{
    public class DetailScheduledDeliveryVM
    {
        public ICollection<DetailScheduledProductVM> Products { get; set; } = new HashSet<DetailScheduledProductVM>();
    }
    
    public class DetailScheduledProductVM
    {
        public int ProductId { get; set; }
        public int ProductDeliveryId { get; set; }
        public string ProductTitle { get; set; }
        public int ShopId { get; set; }
        public int Amount { get; set; }
        public bool Confirmed { get; set; }
        public int Prev { get; set; }
    }
}