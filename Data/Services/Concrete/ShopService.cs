using System;
using System.Linq;
using Base.Services.Abstract;
using Base.Services.Concrete;
using Data.Entities;
using Data.Enums;
using Data.Services.Abstract;

namespace Data.Services.Concrete
{
    public class ShopService : BaseObjectService<Shop>, IShopService
    {
        private readonly ShopContext _context;
        private readonly IInfoMoneyService _infoMoneyService;
        private readonly IBaseObjectService<User> _userService;

        public ShopService(ShopContext context,
            IInfoMoneyService infoMoneyService,
            IBaseObjectService<User> userService) : base(context)
        {
            _context = context;
            _infoMoneyService = infoMoneyService;
            _userService = userService;
        }

        public decimal Turnover(int id)
        {
            var infoMonies = this.infoMonies(id)
                .Where(x => x.Sale.Date.Month == DateTime.Now.Month);
            if (!infoMonies.Any())
                return 0;

            var res = infoMonies
                .Where(x => x.MoneyOperationType != MoneyOperationType.Transfer)
                .Sum(x => x.Sum);

            return Math.Round(res, 2);
        }

        public decimal Margin(int id)
        {
            var sales = this.sales(id)
                .Where(x => x.Payment == true && x.Date.Month == DateTime.Now.Month);
            if (!sales.Any())
                return 0;

            var res = sales.Sum(x => x.Margin);

            return Math.Round(res, 2);
        }

        public decimal DateTurnover(int id, DateTime start, DateTime end)
        {
            var infoMonies = this.infoMonies(id).Where(x => (x.Date >= start && x.Date <= end));
            if (!infoMonies.Any())
                return 0;

            var res = infoMonies.Sum(x => x.Sum);

            return Math.Round(res, 2);
        }

        public decimal DateMargin(int id, DateTime start, DateTime end)
        {
            var sales = this.sales(id).Where(x => x.Date >= start && x.Date <= end);
            if (!sales.Any())
                return 0;

            var res = sales.Sum(x => x.Margin);

            return Math.Round(res, 2);
        }

        public decimal DateAverageMargin(int id, DateTime start, DateTime end)
        {
            var sales = this.sales(id).Where(x => x.Date >= start && x.Date <= end);
            if (!sales.Any())
                return 0;

            var res = sales.Sum(x => x.Margin);
            res /= sales.Count();

            return Math.Round(res, 2);
        }

        public decimal DateAverageCheck(int id, DateTime start, DateTime end)
        {
            var sales = this.sales(id).Where(x => x.Date >= start && x.Date <= end);
            if (!sales.Any())
                return 0;

            var res = sales.Sum(x => x.Sum);
            res /= sales.Count();

            return Math.Round(res, 2);
        }
        
        public decimal CashOnHand(int id)
        {
            var res = _infoMoneyService.GetMoneyWorkerBalance(id);

            return res;
        }

        public Shop ShopByUserId(int id)
        {
            var shopId = _userService.All().First(x => x.Id == id).ShopId;
            var shop = All().FirstOrDefault(x => x.Id == shopId);

            return shop;
        }

        private IQueryable<InfoMoney> infoMonies(int id)
        {
            return _context.InfoMonies.Where(x => x.MoneyWorkerId == id);
        }

        private IQueryable<Sale> sales(int id)
        {
            return _context.Sales.Where(x => x.ShopId == id);
        }
    }
}