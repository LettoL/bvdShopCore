using Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Entities
{
    public class MoneyTransfer : BaseObject
    {
        public int PrevInfoMoneyId { get; set; }
        public InfoMoney PrevInfoMoney { get; set; }

        public int NextInfoMoneyId { get; set; }
        public InfoMoney NextInfoMoney { get; set; }
    }
}
