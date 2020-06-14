namespace Domain.Entities.Olds
{
    public class SaleOld : Entity
    {
        public int SaleOldId { get; private set; }

        public int ManagerId { get; private set; }

        public SaleOld(int saleOldId, int managerId) =>
            (SaleOldId, ManagerId) = (saleOldId, managerId);
    }
}