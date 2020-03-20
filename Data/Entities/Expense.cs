using Base;

namespace Data.Entities
{
    public class Expense : BaseObject
    {
        public int InfoMoneyId { get; set; }
        public InfoMoney InfoMoney { get; set; }

        public int ExpenseCategoryId { get; set; }
        public ExpenseCategory ExpenseCategory { get; set; }

        public int? ShopId { get; set; }
        public Shop Shop { get; set; }
    }
}
