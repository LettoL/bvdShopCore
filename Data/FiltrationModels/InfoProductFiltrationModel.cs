using System;

namespace Data.FiltrationModels
{
    public class InfoProductFiltrationModel
    {
        public DateTime? periodStart { get; set; }
        public DateTime? periodEnd { get; set; }
        public int shopId { get; set; }
        public int type { get; set; }
    }
}