using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Dtos.CloseSale;

namespace WebUI.Dtos
{
    public class CloseSaleDto
    {
        public int SaleId { get; set; }
        public bool Realization { get; set; }
        public int MoneyWorkerId { get; set; }
        public int MoneyWorkerCashlessId { get; set; }
        public int SupplierId { get; set; }
        public IList<SaleProductItem> Products { get; set; }
        public decimal AdditionalCost { get; set; }

    }
}
