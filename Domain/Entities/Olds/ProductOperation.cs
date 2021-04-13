using System;

namespace Domain.Entities.Olds
{
    public class ProductOperation : Entity
    {
        public int ProductId { get; private set; }
        
        public int Amount { get; private set; }

        public DateTime DateTime { get; private set; }
        
        public decimal Cost { get; private set; }
        
        public bool ForRealization { get; private set; }
        
        public int SupplierId { get; private set; }

        public StorageType StorageType { get; private set; }
        
        public ProductOperation(int productId, int amount, DateTime dateTime,
            decimal cost, bool forRealization, int supplierId, StorageType storageType) =>
            (ProductId, Amount, DateTime, Cost, ForRealization, SupplierId, StorageType) 
            = (productId, amount, dateTime, cost, forRealization, supplierId, storageType);
    }

    public enum StorageType
    {
        Shop = 10,
        
        SupplierWarehouse = 20
    }
}