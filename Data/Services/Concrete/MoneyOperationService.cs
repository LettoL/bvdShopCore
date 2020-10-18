using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Base.Services.Abstract;
using Data.Entities;
using Data.Enums;
using Data.Services.Abstract;
using Domain.Entities.Olds;
using PostgresData;

namespace Data.Services.Concrete
{
    public class MoneyOperationService : IMoneyOperationService
    {
        private readonly IInfoMoneyService _infoMoneyService;
        private readonly IBaseObjectService<Expense> _expenseService;
        private readonly IBaseObjectService<MoneyWorker> _moneyWorkerService;

        public MoneyOperationService(IInfoMoneyService infoMoneyService,
            IBaseObjectService<Expense> expenseService,
            IBaseObjectService<MoneyWorker> moneyWorkerService)
        {
            _infoMoneyService = infoMoneyService;
            _expenseService = expenseService;
            _moneyWorkerService = moneyWorkerService;
        }

        public void SalePayment(ShopContext db, decimal cash, decimal cashless, Sale sale,
            int? moneyWorkerIdForCash, int? moneyWorkerIdForCashless)
        {
            if (cash > 0)
                db.InfoMonies.Add(new InfoMoney()
                {
                    MoneyWorkerId = moneyWorkerIdForCash,
                    SaleId = sale.Id,
                    Sum = cash,
                    PaymentType = PaymentType.Cash,
                    MoneyOperationType = MoneyOperationType.Sale,
                    Date = DateTime.Now.AddHours(3)
                });
            if (cashless > 0)
                db.InfoMonies.Add(new InfoMoney()
                {
                    MoneyWorkerId = moneyWorkerIdForCashless,
                    SaleId = sale.Id,
                    Sum = cashless,
                    PaymentType = PaymentType.Cashless,
                    MoneyOperationType = MoneyOperationType.Sale,
                    Date = DateTime.Now.AddHours(3)
                });
        }

        public void Expense(PostgresContext postgresContext, int moneyWorkerId, decimal sum, PaymentType paymentType,
            int categoryId, string comment, int forId)
        {
            var createdInfoMoney = _infoMoneyService.Create(new InfoMoney()
            {
                MoneyWorkerId = moneyWorkerId,
                Sum = -sum,
                MoneyOperationType = MoneyOperationType.Expense,
                PaymentType = paymentType,
                Comment = comment
            });

            var expense = _expenseService.Create(new Expense()
            {
                InfoMoneyId = createdInfoMoney.Id,
                ExpenseCategoryId = categoryId,
            });

            postgresContext.ExpensesOld.Add(new ExpenseOld(expense.Id, forId));
            postgresContext.SaveChanges();
        }

        public void Expense(PostgresContext postgresContext, int moneyWorkerId, decimal sum, PaymentType paymentType,
            int categoryId, string comment, int shopId, int forId)
        {
            var createdInfoMoney = _infoMoneyService.Create(new InfoMoney()
            {
                MoneyWorkerId = moneyWorkerId,
                Sum = -sum,
                MoneyOperationType = MoneyOperationType.Expense,
                PaymentType = paymentType,
                Comment = comment
            });

            var expense = _expenseService.Create(new Expense()
            {
                InfoMoneyId = createdInfoMoney.Id,
                ExpenseCategoryId = categoryId,
                ShopId = shopId
            });

            postgresContext.ExpensesOld.Add(new ExpenseOld(expense.Id, forId));
            postgresContext.SaveChanges();
        }

        public PaymentType PaymentTypeByMoneyWorker(int moneyWorkerId)
        {
            var moneyWorker = _moneyWorkerService.Get(moneyWorkerId);

            if (moneyWorker is Shop)
                return PaymentType.Cash;
            if (moneyWorker is CardKeeper)
                return PaymentType.Cashless;
            if (moneyWorker is CalculatedScore)
                return PaymentType.Cashless;

            throw new Exception("Тип MoneyWorker не определён");
        }

        /*public PaymentType PaymentTypeSaleByInfoMonies(int saleId)
        {
            var saleInfoMoneys = _infoMoneyService.All()
                .Where(s => s.SaleId == saleId)
                .GroupBy(s => s.PaymentType);

            var paymentType = saleInfoMoneys.Count() > 1 ?
                PaymentType.Mixed :
                saleInfoMoneys.SelectMany(group => group)
                    .First()
                    .PaymentType;

            return paymentType;
        }*/
    }
}
