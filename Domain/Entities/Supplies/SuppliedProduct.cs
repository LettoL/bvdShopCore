using System;
using Domain.Commands.Supplies;

namespace Domain.Entities.Supplies
{
    public class SuppliedProduct
    {
        public Guid Id { get; private set; }

        public Guid ProductId { get; private set; }

        public Guid SupplierId { get; private set; }

        public Guid ShopId { get; private set; }

        public int Amount { get; private set; }

        public SupplyType Type { get; private set; }

        public SuppliedProduct(SupplyProduct command)
        {
            Id = command.Id;
            ProductId = command.ProductId;
            SupplierId = command.SupplierId;
            ShopId = command.ShopId;
            Amount = command.Amount;
            Type = command.Type;
        }
        
        private SuppliedProduct(){}
    }

    public enum SupplyType
    {
        Payment = 1,

        ForRealization = 2,

        DeferredPayment = 3
    }
}