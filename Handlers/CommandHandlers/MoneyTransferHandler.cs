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
        public static void MoneyTransfer(MoneyTransferVM moneyTransfer, ShopContext shopContext)
        {
            var prevInfoMoney = new InfoMoney()
            {
                Sum = -moneyTransfer.Sum,
                PaymentType = moneyTransfer.PrevMoneyWorkerType == 3 || moneyTransfer.NextMoneyWorkerType == 3 //Проверка на магазин
                    ? PaymentType.Cash 
                    : PaymentType.Cashless,
                MoneyOperationType = MoneyOperationType.Transfer,
                MoneyWorkerId = moneyTransfer.PrevMoneyWorkerID,
                Date = DateTime.Now.AddHours(3),
            };
            var nextInfoMoney = new InfoMoney()
            {
                Sum = moneyTransfer.Sum,
                PaymentType =
                    moneyTransfer.PrevMoneyWorkerType == 3 ||
                    moneyTransfer.NextMoneyWorkerType == 3 //Проверка на магазин
                        ? PaymentType.Cash
                        : PaymentType.Cashless,
                MoneyOperationType = MoneyOperationType.Transfer,
                MoneyWorkerId = moneyTransfer.NextMoneyWorkerID,
                Date = DateTime.Now.AddHours(3),
            };
            var moneyTransferAdd = new MoneyTransfer()
            {
                PrevInfoMoney = prevInfoMoney,
                NextInfoMoney = nextInfoMoney,
            };
            shopContext.MoneyTransfers.Add(moneyTransferAdd);
            shopContext.SaveChanges();
        }
    }
}