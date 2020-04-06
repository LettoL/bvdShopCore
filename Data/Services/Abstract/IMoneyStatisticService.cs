using System.Linq;
using Data.ViewModels;

namespace Data.Services.Abstract
{
    public interface IMoneyStatisticService
    {
        IQueryable<ExpenseListVM> Expenses(ShopContext context);
        IQueryable<ExpenseListVM> ShopExpenses(ShopContext db, int shopId);

        decimal DailyProfit(ShopContext context);
    }
}