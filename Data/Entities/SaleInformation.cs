using System;
using System.Collections.Generic;
using System.Text;
using Base;
using Data.Enums;

namespace Data.Entities
{
    public class SaleInformation : BaseObject
    {
        public int SaleId { get; set; }
        public Sale Sale { get; set; }

        public int? MoneyWorkerForIncomeId { get; set; }
        public MoneyWorker MoneyWorkerForIncome { get; set; }

        public int? MoneyWorkerForCashlessIncomeId { get; set; }
        public MoneyWorker MoneyWorkerForCashlessIncome { get; set; }

        public int? MoneyWorkerForExpenseId { get; set; }
        public MoneyWorker MoneyWorkerForExpense { get; set; }

        public SaleType SaleType { get; set; }
    }
}
