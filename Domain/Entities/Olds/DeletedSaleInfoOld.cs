using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Olds
{
    public class DeletedSaleInfoOld : Entity
    {
        [Column(TypeName = "jsonb")]
        public DeletedSale Sale { get; set; }
    }

    public class DeletedSale
    {
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public DateTime? DeletedDate { get; set; }
        public decimal Sum { get; set; }
        public decimal Discount { get; set; }
        public decimal Margin { get; set; }
        public decimal ProcurementCost { get; set; }
        public int ShopId { get; set; }
        public string ShopTitle { get; set; }
        public string Buyer { get; set; }
        public string SaleType { get; set; }
        public ICollection<DeletedSaleProduct> Products { get; set; } = new HashSet<DeletedSaleProduct>();
        public ICollection<DeletedPayment> Payments { get; set; } = new HashSet<DeletedPayment>();
    }

    public class DeletedSaleProduct
    {
        public int ProductId { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public decimal ProcurementCost { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
    }

    public class DeletedPayment
    {
        public string Account { get; set; }
        public decimal Sum { get; set; }
        public string Comment { get; set; }
        public string PaymentType { get; set; }
        public DateTime Date { get; set; }
    }
}