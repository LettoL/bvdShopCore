using System;

namespace Domain.Entities.Olds
{
    public class SupplierPayment : Entity
    {
        public decimal Sum { get; private set; }

        public int SupplierId { get; private set; }

        public DateTime DateTime { get; private set; }

        public SupplierPayment(decimal sum, int supplierId, DateTime dateTime) =>
            (Sum, SupplierId, DateTime) = (sum, supplierId, dateTime);
    }
}