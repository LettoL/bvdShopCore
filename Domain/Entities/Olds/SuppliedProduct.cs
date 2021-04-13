using System;

namespace Domain.Entities.Olds
{
    public class SuppliedProduct
    {
        public int SupplierId { get; private set; }

        public int ProductOperationId { get; private set; }
        public ProductOperation ProductOperation { get; private set; }
        
        public decimal Cost { get; private set; }
    }

    public enum SuppliedProductType
    {
        Paid = 10,
        
        ForRealization = 20,
        
        Deferred = 30
    }
}