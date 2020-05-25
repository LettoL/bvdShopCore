using System;

namespace Application.Entities
{
    public class SuppliedProduct : Entity
    {
        public DateTime Date { get; private set; }
        public int SuppliedAmount { get; private set; }
        public int StockAmount { get; private set; }
        public decimal ProcurementCost { get; private set; }
        public SuppliedType Type { get; private set; }
        
        public int ShopId { get; private set; }
        public Shop Shop { get; private set; }
        
        public int ProductId { get; private set; }
        public Product Product { get; private set; }

        public SuppliedProduct(
            DateTime date, int suppliedAmount, decimal procurementCost, int shopId, int productId, SuppliedType type) =>
            (Date, SuppliedAmount, ProcurementCost, ShopId, ProductId, Type) =
            (date, suppliedAmount, procurementCost, shopId, productId, type);
    }

    public enum SuppliedType
    {
        Payment = 10,
        
        ForRealization = 20,
        
        DeferredPayment = 30
    }
}