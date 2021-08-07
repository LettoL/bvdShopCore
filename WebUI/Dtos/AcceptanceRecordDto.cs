using System;
using System.Collections.Generic;

namespace WebUI.Dtos
{
    public class AcceptanceRecordDto
    {
        public ICollection<AcceptanceRecordDate> Dates { get; set; } = new HashSet<AcceptanceRecordDate>();
    }

    public class AcceptanceRecordDate
    {
        public string Date { get; set; }
        public ICollection<AcceptanceRecordSupplied> Supplieds { get; set; } = new HashSet<AcceptanceRecordSupplied>();
        public ICollection<AcceptanceRecordPayment> Payments { get; set; } = new HashSet<AcceptanceRecordPayment>();
    }

    public class AcceptanceRecordSupplied
    {
        public string ProductTitle { get; set; }
        public int Amount { get; set; }
        public decimal PriceOfUnit { get; set; }
        public decimal PriceSum { get; set; }
    }

    public class AcceptanceRecordPayment
    {
        public decimal Sum { get; set; }
    }
}