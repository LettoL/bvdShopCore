namespace Domain.Entities.Olds
{
    public class RepaidDebtOld : Entity
    {
        public int SupplierId { get; private set; }
        public int InfoMoneyId { get; private set; }

        public RepaidDebtOld(int supplierId, int infoMoneyId) =>
            (SupplierId, InfoMoneyId) = (supplierId, infoMoneyId);
    }
}