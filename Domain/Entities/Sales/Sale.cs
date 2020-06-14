using System;
using System.Collections.Generic;

namespace Domain.Entities.Sales
{
    public class Sale : Entity
    {
        public DateTime Date { get; private set; }

        public decimal Sum { get; private set; }

        public int ManagerId { get; private set; }
        public Manager Manager { get; private set; }

        public ICollection<SoldProduct> SoldProducts { get; private set; } = new HashSet<SoldProduct>();
    }
}