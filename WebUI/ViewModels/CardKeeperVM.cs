using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.ViewModels
{
    public class CardKeeperVM
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string CardNumber { get; set; }

        public bool ForManager { get; set; }

        public decimal Balance { get; set; }
    }
}
