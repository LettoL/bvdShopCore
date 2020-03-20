using System;
using Data.Enums;

namespace Data.ViewModels
{
    public class SupplyProductVM
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public int ShopId { get; set; }
        public int SupplierId { get; set; }
        public SupplyType Realization { get; set; }
        public DateTime? Date { get; set; }
        public decimal AdditionalCost { get; set; }
        public decimal ProcurementCost { get; set; }       
    }
}
