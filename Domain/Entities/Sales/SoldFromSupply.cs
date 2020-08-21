using System;

namespace Domain.Entities.Sales
{
    public class SoldFromSupply
    {
        public Guid Id { get; private set; }

        public Guid SaleProductId { get; private set; }

        public Guid SuppliedProductId { get; private set; }

        public decimal ProcurementCost { get; private set; }

        public int Amount { get; private set; }
    }
}