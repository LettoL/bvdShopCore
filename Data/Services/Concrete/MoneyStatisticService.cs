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

        public IQueryable<ExpenseListVM> Expenses()
        {
            return _expenseService.All()
                .Where(x => x.InfoMoney.Date.Date == DateTime.Now.Date.Date)
                .Select(x => new ExpenseListVM()
                {
                    Date = x.InfoMoney.Date.ToString("dd.MM.yyyy"),
                    Comment = x.InfoMoney.Comment,
                    MoneyWorker = x.InfoMoney.MoneyWorker.Title,
                    Sum = Math.Abs(x.InfoMoney.Sum)
                });
        }

        public IQueryable<ExpenseListVM> ShopExpenses(int shopId)
        {
            return _expenseService.All()
                .Where(x => x.ShopId == shopId && x.InfoMoney.Date.Date == DateTime.Now.Date)
                .Select(x => new ExpenseListVM()
                {
                    Date = x.InfoMoney.Date.ToString("dd.MM.yyyy"),
                    Comment = x.InfoMoney.Comment,
                    MoneyWorker = x.InfoMoney.MoneyWorker.Title,
                    Sum = Math.Abs(x.InfoMoney.Sum)
                });
        }

        public decimal DailyProfit()
        {
            return _infoMoneyService.All()
                .Where(im => im.Date.DayOfYear == DateTime.Now.DayOfYear
                             && im.Date.Year == DateTime.Now.Year
                             && im.Sum > 0
                             && im.MoneyOperationType != MoneyOperationType.Transfer
                             && im.MoneyOperationType != MoneyOperationType.Expense
                             && im.MoneyOperationType != MoneyOperationType.SupplierRepayment)
                .Sum(im => im.Sum);
        }
    }
}