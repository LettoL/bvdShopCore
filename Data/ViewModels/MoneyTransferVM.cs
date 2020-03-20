using System;
using System.Collections.Generic;
using System.Text;

namespace Data.ViewModels
{
    public class MoneyTransferVM
    {
        public int PrevMoneyWorkerID { get; set; }
        public int PrevMoneyWorkerType { get; set; }
        public int NextMoneyWorkerID { get; set; }
        public int NextMoneyWorkerType { get; set; }
        public decimal Sum { get; set; }
    }
}
