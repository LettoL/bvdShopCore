using System;
using System.Collections.Generic;

namespace Domain.Commands.Sales
{
    public class SaleCreate
    {
        public Guid ShopId { get; set; }
        public DateTime DateTime { get; set; }
        public decimal CashSum { get; set; }
        public decimal CashlessSum { get; set; }
        public Guid ManagerId { get; set; }
        public Guid? BuyerId { get; set; }
        public decimal Discount { get; set; }
        public Guid? CashlessAccountId { get; set; }
        public bool ForRf { get; set; }
        public ICollection<SellingProduct> SellingProducts { get; set; } = new HashSet<SellingProduct>();
    }

    public class SellingProduct
    {
        public Guid ProductId { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
    }

    public class MoneyToPay
    {
        public decimal Sum { get; set; }
        public Guid AccountId { get; set; }
    }
}