using System;

namespace Data.FiltrationModels
{
    public class SaleFiltrationModel
    {
        public int shopId { get; set; }
        public int type { get; set; }
        public int buyer { get; set; }
        public bool forRF { get; set; }
        public string periodStart { get; set; }
        public string periodEnd { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
    }
}

