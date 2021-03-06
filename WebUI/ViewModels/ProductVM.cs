﻿using Data.Entities;

namespace WebUI.ViewModels
{
    public class ProductVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public int Amount { get; set; }
        public int BookedCount { get; set; }
        public decimal ProcurementCost { get; set; }
        public decimal AdditionalCost { get; set; }
        public decimal FinalCost { get; set; }
        public decimal Cost { get; set; }
        public Shop Shop { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }
    }
}
