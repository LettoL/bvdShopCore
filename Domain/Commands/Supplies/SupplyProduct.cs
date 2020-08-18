using System;
using Domain.Entities.Supplies;

namespace Domain.Commands.Supplies
{
    public class SupplyProduct
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid SupplierId { get; set; }
        public Guid ShopId { get; set; }
        public int Amount { get; set; }
        public SupplyType Type { get; set; }
    }
}