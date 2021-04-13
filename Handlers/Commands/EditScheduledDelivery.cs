using System.Collections.Generic;

namespace Handlers.Commands
{
    public class EditScheduledDelivery
    {
        public int DeliveryId { get; set; }
        public ICollection<EditedScheduledDeliveryProduct> Products { get; set; } =
            new HashSet<EditedScheduledDeliveryProduct>();
    }

    public class EditedScheduledDeliveryProduct
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