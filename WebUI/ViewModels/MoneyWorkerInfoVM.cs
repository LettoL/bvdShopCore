using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.ViewModels
{
    public class MoneyWorkerInfoVM
    {
        public string Title { get; set; }
        public decimal MorningCash { get; set; }
        public decimal EveningCash { get; set; }
    }
}
