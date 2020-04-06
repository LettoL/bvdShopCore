using Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.ViewModels
{
    public class SaleByCategoryVM
    {
        public Category Category { get; set; }
        public int SalesByMoscow { get; set; }
        public int SalesByPetersburg { get; set; }
        public int SalesBySamara { get; set; }
        public int ForRussianFederation { get; set; }
        public int PartnerSales { get; set; }
        public decimal TurnOver { get; set; }
        public decimal TurnOverMoscow { get; set; }
        public decimal TurnOverPetersburg { get; set; }
        public decimal TurnOverSamara { get; set; }
        public decimal TurnOverRF { get; set; }
        public decimal TurnOverPartner { get; set; }
        public decimal Margin { get; set; }
        public decimal MarginMoscow { get; set; }
        public decimal MarginPetersburg { get; set; }
        public decimal MarginSamara { get; set; }
        public decimal MarginRF { get; set; }
        public decimal MarginPartner { get; set; }
    }
}
