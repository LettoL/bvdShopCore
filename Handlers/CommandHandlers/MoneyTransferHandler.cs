using System;
using Data;
using Data.Entities;
using Data.Enums;
using Data.ViewModels;
using PostgresData;

namespace Handlers.CommandHandlers
{
    public static class MoneyTransferHandler
    {
        public static void MoneyTransfer(MoneyTransferVM moneyTransfer, PostgresContext postgresContext, ShopContext shopContext)
        {
            var prevInfoMoney = shopContext.InfoMonies.Add(new InfoMoney()
            {
                Sum = -moneyTransfer.Sum,
                PaymentType = moneyTransfer.PrevMoneyWorkerType == 3 || moneyTransfer.NextMoneyWorkerType == 3 //Проверка на магазин
                    ? PaymentType.Cash 
                    : PaymentType.Cashless,
                MoneyOperationType = MoneyOperationType.Transfer,
                MoneyWorkerId = moneyTransfer.PrevMoneyWorkerID,
                Date = DateTime.Now.AddHours(3),
            });
            var nextInfoMoney = shopContext.InfoMonies.Add(new InfoMoney()
                {
                    Sum = moneyTransfer.Sum,
                    PaymentType = moneyTransfer.PrevMoneyWorkerType == 3 || moneyTransfer.NextMoneyWorkerType == 3 //Проверка на магазин
                        ? PaymentType.Cash
                        : PaymentType.Cashless,
                    MoneyOperationType = MoneyOperationType.Transfer,
                    MoneyWorkerId = moneyTransfer.NextMoneyWorkerID,
                    Date = DateTime.Now.AddHours(3),
            });
            shopContext.SaveChanges();
            var prevId = prevInfoMoney.Entity.Id;
            var nextId = nextInfoMoney.Entity.Id;
            shopContext.MoneyTransfers.Add( new MoneyTransfer()
            {
                PrevInfoMoneyId = prevId,
                NextInfoMoneyId = nextId
            });
        }
    }
}