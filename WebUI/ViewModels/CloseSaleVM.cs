using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.ViewModels
{
    public class CloseSaleVM
    {
        public int? MoneyWorkerIdForIncome { get; set; }
        public int? MoneyWorkerIdForExpense { get; set; }
        public int? MoneyWorkerIdForCashlessIncome { get; set; }
        public string MoneyWorkerTitleForIncome { get; set; }
        public string MoneyWorkerTitleForExpense { get; set; }
        public string MoneyWorkerTitleForCashlessIncome { get; set; }
    }
}
