using System.Linq;
using Data.ViewModels;

namespace Data.Services.Abstract
{
    public interface IMoneyStatisticService
    {
        IQueryable<ExpenseListVM> Expenses();
        IQueryable<ExpenseListVM> ShopExpenses(int shopId);

        decimal DailyProfit();
    }
}