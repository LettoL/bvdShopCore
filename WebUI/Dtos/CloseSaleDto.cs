using System.Collections.Generic;
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
        public ICollection<SaleProductItem> Products { get; set; } = new HashSet<SaleProductItem>();
        public decimal AdditionalCost { get; set; }
    }
}
