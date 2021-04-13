namespace Domain.Entities.Olds
{
    public class SupplierInfoInit : Entity
    {
        public int SupplierId { get; private set; }
        public decimal Debt { get; private set; }
        public decimal PriceProductsForRealization { get; private set; }
        public decimal PriceProducts { get; private set; }

        public SupplierInfoInit(int supplierId, decimal debt, decimal priceProductsForRealization,
            decimal priceProducts) =>
            (SupplierId, Debt, PriceProductsForRealization, PriceProducts)
            = (supplierId, debt, priceProductsForRealization, priceProducts);
    }
}