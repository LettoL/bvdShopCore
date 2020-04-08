using System;
using System.Linq;
using Base.Services.Abstract;
using Data.Entities;
using Data.Enums;
using Data.Services.Abstract;
using Data.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Data.Services.Concrete
{
    public class MoneyStatisticService : IMoneyStatisticService
    {
        private readonly IBaseObjectService<Expense> _expenseService;
        private readonly IInfoMoneyService _infoMoneyService;

        public MoneyStatisticService(IBaseObjectService<Expense> expenseService,
            IInfoMoneyService infoMoneyService)
        {
            _expenseService = expenseService;
            _infoMoneyService = infoMoneyService;
        }

        public IQueryable<ExpenseListVM> Expenses(ShopContext context)
        {
            return context.Expenses
                .Where(x => x.InfoMoney.Date.Date == DateTime.Now.AddHours(3).Date.Date)
                .Select(x => new ExpenseListVM()
                {
                    Date = x.InfoMoney.Date.ToString("dd.MM.yyyy"),
                    Comment = x.InfoMoney.Comment,
                    MoneyWorker = x.InfoMoney.MoneyWorker.Title,
                    Sum = Math.Abs(x.InfoMoney.Sum)
                });
        }

        public IQueryable<ExpenseListVM> ShopExpenses(ShopContext db, int shopId)
        {
            return db.Expenses
                .Where(x => x.ShopId == shopId && x.InfoMoney.Date.Date == DateTime.Now.AddHours(3).Date)
                .Select(x => new ExpenseListVM()
                {
                    Date = x.InfoMoney.Date.ToString("dd.MM.yyyy"),
                    Comment = x.InfoMoney.Comment,
                    MoneyWorker = x.InfoMoney.MoneyWorker.Title,
                    Sum = Math.Abs(x.InfoMoney.Sum)
                });
        }

        public decimal DailyProfit(ShopContext context)
        {
            return context.InfoMonies
                .Where(im => im.Date.DayOfYear == DateTime.Now.AddHours(3).DayOfYear
                             && im.Date.Year == DateTime.Now.AddHours(3).Year
                             && im.Sum > 0
                             && im.MoneyOperationType != MoneyOperationType.Transfer
                             && im.MoneyOperationType != MoneyOperationType.Expense
                             && im.MoneyOperationType != MoneyOperationType.SupplierRepayment)
                .Sum(im => im.Sum);
        }
    }
}