using Domain.Entities;
using Domain.Entities.Products;
using Domain.Entities.Supplies;

namespace WebUI.Commands
{
    public class SupplyProduct
    {
        public int ProductId { get; set; }
        public int ShopId { get; set; }
        public int SupplierId { get; set; }
        public int Amount { get; set; }
        public string ProcurementCost { get; set; }
        public SuppliedType Type { get; set; }
    }
}