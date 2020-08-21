using System;
using System.Collections.Generic;

namespace Domain.Entities.Sales
{
    public class Sale
    {
        public Guid Id { get; private set; }

        public Guid ShopId { get; private set; }

        public DateTime DateTime { get; private set; }

        public bool ForRf { get; private set; }

        public decimal Discount { get; private set; }

        public ICollection<SoldProduct> SoldProducts { get; private set; } = new HashSet<SoldProduct>();
        
        public ICollection<DepositedMoney> DepositedMonies { get; private set; } = new HashSet<DepositedMoney>();
        
        
    }
}