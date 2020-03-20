using System;

namespace Data.ViewModels
{
    public class ExpenseListVM
    {
        public string Date { get; set; }
        public decimal Sum { get; set; }
        public string MoneyWorker { get; set; }
        public string Comment { get; set; }
    }
}