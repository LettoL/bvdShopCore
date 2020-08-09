using System;
using Domain.Commands;
using Domain.Entities.Supplies;

namespace Domain.Entities.Products
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

        public int SupplierId { get; private set; }
        public Supplier Supplier { get; private set; }

        public SuppliedProduct(
            DateTime date, int suppliedAmount, decimal procurementCost,
            int shopId, int productId, SuppliedType type, int supplierId)
        {
            Date = date;
            SuppliedAmount = suppliedAmount;
            StockAmount = suppliedAmount;
            ProcurementCost = procurementCost;
            ShopId = shopId;
            ProductId = productId;
            Type = type;
            SupplierId = supplierId;
        }

        public static SuppliedProduct Create(SupplyProduct supplyProduct, DateTime dateTime)
        {
            return new SuppliedProduct(
                dateTime,
                supplyProduct.Amount,
                supplyProduct.ProcurementCost,
                supplyProduct.ShopId,
                supplyProduct.ProductId,
                supplyProduct.Type,
                supplyProduct.SupplierId);
        }

        public int ReduceStockAmount(int amount)
        {
            if (StockAmount < amount) throw new Exception("Товара на складе не достаточно для операции");

            StockAmount -= amount;

            return StockAmount;
        }
    }

    public enum SuppliedType
    {
        Payment = 10,
        
        ForRealization = 20,
        
        DeferredPayment = 30
    }
}