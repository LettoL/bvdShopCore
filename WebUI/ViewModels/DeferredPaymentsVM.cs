using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.ViewModels
{
    public class DeferredPaymentsVM
    {
        public string Date { get; set; }
        public decimal Sum { get; set; }
        public string SupplierTitle { get; set; }
    }
}
