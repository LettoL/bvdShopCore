using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Entities;
using Data.Enums;

namespace WebUI.ViewModels
{
    public class MoneyHistoryVM
    {
        public int Id { get; set; }
        public decimal Sum { get; set; }
        public string Date { get; set; }
        public string Comment { get; set; }
        public PaymentType PaymentType { get; set; }
        public Sale Sale { get; set; }
        public MoneyWorker MoneyWorker { get; set; }
        public MoneyOperationType MoneyOperationType { get; set; }
        public string ShopTitle { get; set; }
    }
}
