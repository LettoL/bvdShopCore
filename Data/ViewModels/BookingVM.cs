using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.ViewModels
{
    public class BookingVM
    {
        public IList<BookingCreateVM> Products { get; set; }
        public decimal Sum { get; set; }
        public decimal CashSum { get; set; }
        public decimal CashlessSum { get; set; }
        public decimal Discount { get; set; }
        public int? MoneyWorkerId { get; set; }
        public int UserId { get; set; }
        public int BookingType { get; set; }
        public string Buyer { get; set; }
        public bool forRussian { get; set; }
        public string Manager { get; set; }
    }

    public class BookingCreateVM
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public bool Additional { get; set; }
        public decimal Cost { get; set; }
    }
}
