using System;

namespace Data.FiltrationModels
{
    public class SaleFiltrationModel
    {
        public int shopId { get; set; }
        public int type { get; set; }
        public int buyer { get; set; }
        public bool forRF { get; set; }
        public DateTime? periodStart { get; set; }
        public DateTime? periodEnd { get; set; }
    }
}

