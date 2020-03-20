using System;
using Base;
using Data.Enums;

namespace Data.Entities
{
    public class InfoMoney : BaseObject
    {
        public decimal Sum { get; set; }

        public int? MoneyWorkerId { get; set; }
        public MoneyWorker MoneyWorker { get; set; }

        public int? SaleId { get; set; }
        public Sale Sale { get; set; }

        public int? BookingId { get; set; }
        public Booking Booking { get; set; }

        public string Comment { get; set; }

        public DateTime Date { get; set; }

        public PaymentType PaymentType { get; set; }

        public MoneyOperationType MoneyOperationType { get; set; }
    }
}
