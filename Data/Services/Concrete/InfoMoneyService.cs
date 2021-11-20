using System;
using System.Linq;
using Base.Services.Abstract;
using Base.Services.Concrete;
using Data.Entities;
using Data.Enums;
using Data.Services.Abstract;
using Data.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Data.Services.Concrete
{
    public class InfoMoneyService : BaseObjectService<InfoMoney>, IInfoMoneyService
    {
        private readonly IBaseObjectService<MoneyTransfer> _moneyTransferService;

        public InfoMoneyService(ShopContext context, 
            IBaseObjectService<MoneyTransfer> moneyTransferService) : base(context)
        {
            _moneyTransferService = moneyTransferService;
        }

        public override InfoMoney Create(InfoMoney obj)
        {
            obj.Date = DateTime.Now.AddHours(3);

            return base.Create(obj);
        }

        public decimal GetMoneyWorkerBalance(ShopContext db, int moneyWorkerId)
        {
            var balance = db.InfoMonies.Where(im => im.MoneyWorkerId == moneyWorkerId)
                .Sum(im => im.Sum);

            return balance;
        }

        public decimal Balance()
        {
            return this.All()
                .Where(x => x.MoneyOperationType != MoneyOperationType.Transfer
                            && x.MoneyWorkerId != null)
                .Sum(x => x.Sum);
        }

        public decimal SaleSum(int id)
        {
            return this.All().Where(x => x.SaleId == id).Sum(x => x.Sum);
        }
    }
}