using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Base;

namespace Data.Entities
{
    public class BookingProduct : BaseObject
    {       
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int BookingId { get; set; }
        public Booking Booking { get; set; }
       
        public int Amount { get; set; }
        public decimal Cost { get; set; }

        public bool Additional { get; set; }
    }
    
}