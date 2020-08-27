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

        public Sale(Guid id, Guid shopId, DateTime dateTime, bool forRf, decimal discount,
            ICollection<SoldProduct> soldProducts, ICollection<DepositedMoney> depositedMonies)
        {
            Id = id;
            ShopId = shopId;
            DateTime = dateTime;
            ForRf = forRf;
            Discount = discount;
            SoldProducts = soldProducts;
            DepositedMonies = depositedMonies;
        }
        
        private Sale() {}
    }
}