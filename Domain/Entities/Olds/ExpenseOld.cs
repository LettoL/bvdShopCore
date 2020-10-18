namespace Domain.Entities.Olds
{
    public class ExpenseOld : Entity
    {
        public int ExpenseId { get; private set; }
        
        public int ForId { get; private set; }

        public ExpenseOld(int expenseId, int forId) =>
            (ExpenseId, ForId) = (expenseId, forId);
    }
}