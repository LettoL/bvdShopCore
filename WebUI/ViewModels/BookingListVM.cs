using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Enums;

namespace WebUI.ViewModels
{
    public class BookingListVM
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string ProductTitle { get; set; }

        public decimal Sum { get; set; }
        public decimal Pay { get; set; }
        public decimal Debt { get; set; }

        public BookingStatus Status { get; set; }
    }
}
