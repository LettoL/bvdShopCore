using System;

namespace Domain.Entities
{
    public class ScheduledProductDelivery : Entity
    {
        public int ProductId { get; set; }

        public int Amount { get; set; }

        public int SupplierId { get; set; }

        public decimal ProcurementCost { get; set; }

        public DateTime CreatedDate { get; set; }

        public int ShopId { get; set; }

        public ScheduledProductDeliveryType DeliveryType { get; set; }
        
        public int ScheduledDeliveryId { get; set; }
        public ScheduledDelivery ScheduledDelivery { get; set; }
        
        public int? SupplyProductId { get; set; }

        public ScheduledProductDelivery(int productId, int amount, int supplierId, decimal procurementCost)
        {
            ProductId = productId;
            Amount = amount;
            SupplierId = supplierId;
            ProcurementCost = procurementCost;
            CreatedDate = DateTime.Now.AddHours(3);
            DeliveryType = ScheduledProductDeliveryType.Scheduled;
        }

        public ScheduledProductDelivery(int productId, int amount, int supplierId,
            decimal procurementCost, int scheduledDeliveryId)
        {
            ProductId = productId;
            Amount = amount;
            SupplierId = supplierId;
            ProcurementCost = procurementCost;
            CreatedDate = DateTime.Now.AddHours(3);
            DeliveryType = ScheduledProductDeliveryType.Scheduled;
            ScheduledDeliveryId = scheduledDeliveryId;
        }
        
        private ScheduledProductDelivery(){}
    }

    public enum ScheduledProductDeliveryType
    {
        Scheduled = 10,
        
        Delivered = 20
    }
}