namespace Domain.Entities.Olds
{
    public class SaleManagerOld : Entity
    {
        public int ManagerId { get; private set; }
        public int SaleId { get; private set; }

        public SaleManagerOld(int managerId, int saleId) =>
            (ManagerId, SaleId) = (managerId, saleId);
    }
}