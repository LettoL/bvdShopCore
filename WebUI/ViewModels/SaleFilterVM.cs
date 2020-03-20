using Data.FiltrationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.ViewModels
{
    public class SaleFilterVM
    {
        public SaleFiltrationModel SaleFiltrationModel { get; set; }
        public int UserId { get; set; }
    }
}
