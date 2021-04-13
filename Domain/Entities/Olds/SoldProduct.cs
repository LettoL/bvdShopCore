namespace Domain.Entities.Olds
{
    public class SoldProduct : Entity
    {
        public int SaleId { get; private set; }
        
        public int ProductOperationId { get; private set; }
        public ProductOperation ProductOperation { get; private set; }

        public int SuppliedProductId { get; private set; }
        public SuppliedProduct SuppliedProduct { get; private set; }

        public SoldProduct(int saleId, ProductOperation productOperation, int suppliedProductId) =>
            (SaleId, ProductOperation, SuppliedProductId) = (saleId, productOperation, suppliedProductId);
    }
}