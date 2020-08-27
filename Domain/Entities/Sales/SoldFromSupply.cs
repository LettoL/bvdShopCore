using System;

namespace Domain.Entities.Sales
{
    public class SoldFromSupply
    {
        public Guid Id { get; private set; }

        public Guid SoldProductId { get; private set; }

        public Guid SuppliedProductId { get; private set; }

        public decimal ProcurementCost { get; private set; }

        public int Amount { get; private set; }

        public SoldFromSupply(Guid id, Guid soldProductId, Guid suppliedProductId, decimal procurementCost, int amount)
        {
            Id = id;
            SoldProductId = soldProductId;
            SuppliedProductId = suppliedProductId;
            ProcurementCost = procurementCost;
            Amount = amount;
        }

        private SoldFromSupply()
        {
            
        }
    }
}