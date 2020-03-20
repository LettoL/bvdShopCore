using Base;
using Data.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Data.Entities
{
    public class Booking : BaseObject
    {
        public DateTime Date { get; set; }
        
        public int UserId { get; set; }

        public int? ShopId { get; set; }
        public Shop Shop { get; set; }

        public int? PartnerId { get; set; }
        public Partner Partner { get; set; }

        public decimal CashSum { get; set; }
        public decimal CashlessSum { get; set; }
        public decimal Sum { get; set; }
        public decimal Pay { get; set; }
        public decimal Debt { get; set; }
        public decimal Discount { get; set; }

        public bool forRussian { get; set; }

        public BookingStatus Status { get; set; }

    }
}