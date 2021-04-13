using System;

namespace WebUI.ViewModels
{
    public class ManagerPaymentVM
    {
        public string ManagerName { get; set; }
        public DateTime DateTime { get; set; }
        public decimal Sum { get; set; }
        public string Comment { get; set; }
    }
}