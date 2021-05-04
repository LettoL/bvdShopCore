using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Enums;

namespace WebUI.ViewModels
{
    public class BookingListItemVM
    {
        public int Id { get; set; }

        public string Date { get; set; }
        public string ProductTitle { get; set; }
        public int ShopId { get;set; }
        public string ShopTitle { get;set; }
        public decimal Sum { get; set; }
        public decimal Pay { get; set; }
        public decimal Debt { get; set; }

        public BookingStatus Status { get; set; }
    }
}
