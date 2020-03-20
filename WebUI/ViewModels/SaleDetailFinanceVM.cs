using System.Collections.Generic;

namespace WebUI.ViewModels
{
    public class SaleDetailFinanceVM
    {
        public IEnumerable<SaleDetailFinanceItemVM> CashScores { get; set; }
        public IEnumerable<SaleDetailFinanceItemVM> CashlessScores { get; set; }
    }

    public class SaleDetailFinanceItemVM
    { 
        public string MoneyWorkerTitle { get; set; }
        public string Sum { get; set; }
    }
}
