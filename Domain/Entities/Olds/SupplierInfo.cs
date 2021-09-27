namespace Domain.Entities.Olds
{
    public class SupplierInfo : Entity
    {
        public int SupplierId { get; set; }

        public bool Removed { get; set; } = false;

        public int Order { get; set; }

        public SupplierInfo(int supplierId, int order)
        {
            SupplierId = supplierId;
            Order = order;
        }

        public void Remove()
        {
            Removed = true;
        }
    }
}