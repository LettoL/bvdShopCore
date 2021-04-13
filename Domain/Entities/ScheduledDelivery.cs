using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class ScheduledDelivery : Entity
    {
        public int SupplierId { get; set; }

        public decimal DepositedSum { get; set; }

        public DateTime CreatedDate { get; set; }

        public ICollection<ScheduledProductDelivery> Products { get; set; }

        public ScheduledDelivery(int supplierId, decimal depositedSum, ICollection<ScheduledProductDelivery> products)
        {
            SupplierId = supplierId;
            DepositedSum = depositedSum;
            CreatedDate = DateTime.Now.AddHours(3);
            Products = products;
        }
        
        private ScheduledDelivery(){}
    }
}