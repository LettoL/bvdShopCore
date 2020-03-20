using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.ViewModels
{
    public class SalesByCategoriesTotalsVM
    {
        public int SumSalesByMoscow { get; set; }
        public int SumSalesByPetersburg { get; set; }
        public int SumChecksByMoscow { get; set; }
        public int SumChecksByPetersburg { get; set; }
        public int SumChecksByPartners { get; set; }
        public int SumChecksByRF { get; set; }
        public int SumForRussianFederation { get; set; }
        public int SumPartnerSales { get; set; }
        public decimal SumMargin { get; set; }
        public decimal SumTurnOver { get; set; }
        public decimal SumTurnOverMoscow { get; set; }
        public decimal SumTurnOverPetersburg { get; set; }
        public decimal SumTurnOverRF { get; set; }
        public decimal SumTurnOverPartner { get; set; }
        public decimal SumMarginMoscow { get; set; }
        public decimal SumMarginPetersburg { get; set; }
        public decimal SumMarginPartner { get; set; }
        public decimal SumMarginRF { get; set; }
    }
}
