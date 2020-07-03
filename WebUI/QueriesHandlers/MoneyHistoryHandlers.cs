using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Microsoft.EntityFrameworkCore;
using PostgresData;
using WebUI.ViewModels;

namespace WebUI.QueriesHandlers
{
    public static class MoneyHistoryHandlers
    {
        public static ICollection<MoneyHistoryVM> GetMoneyHistory(
            PostgresContext postgresContext,
            ShopContext shopContext)
        {
            var repaidDebts = DebtRepaymentOperations(postgresContext, shopContext);

            var repaidDebtsIds = repaidDebts.Select(x => x.Item1).ToList();
            
            var result = shopContext.InfoMonies
                .OrderByDescending(im => im.Id)
                .Take(200)
                .Select(im => new MoneyHistoryVM()
                {
                    Id = im.Id,
                    Sum = im.Sum,
                    Date = im.Date.ToString("dd.MM.yyyy"),
                    Comment = im.Comment,
                    PaymentType = im.PaymentType,
                    MoneyWorkerTitle = im.MoneyWorker != null ? im.MoneyWorker.Title : "",
                    MoneyOperationType = im.MoneyOperationType,
                    ShopTitle = im.Sale.Shop.Title
                })
                .ToList()
                .Select(im => new MoneyHistoryVM()
                {
                    Id = im.Id,
                    Sum = im.Sum,
                    Date = im.Date,
                    Comment = repaidDebtsIds.Contains(im.Id)
                        ? repaidDebts.FirstOrDefault(r
                            => r.Item1 == im.Id).Item2
                        : im.Comment,
                    PaymentType = im.PaymentType,
                    MoneyWorkerTitle = im.MoneyWorkerTitle,
                    MoneyOperationType = im.MoneyOperationType,
                    ShopTitle = im.ShopTitle
                }).ToList();

            return result;
        }

        public static ICollection<MoneyHistoryVM> GetMoneyHistory(
            PostgresContext postgresContext,
            ShopContext shopContext,
            MoneyHistoryFilterQuery query)
        {
            var repaidDebts = DebtRepaymentOperations(postgresContext, shopContext);

            var repaidDebtsIds = repaidDebts.Select(x => x.Item1).ToList();

            var allInfoMoneys = shopContext.InfoMonies
                .AsQueryable();
            
            if (query.StartDate != null)
            {
                var buf = query.StartDate.Split('.');
                var date = new DateTime(
                    Convert.ToInt32(buf[2]),
                    Convert.ToInt32(buf[1]),
                    Convert.ToInt32(buf[0]));
                
                allInfoMoneys = allInfoMoneys.Where(x => x.Date.Date >= date);
            }

            if (query.EndDate != null)
            {
                var buf = query.EndDate.Split('.');
                var date = new DateTime(
                    Convert.ToInt32(buf[2]),
                    Convert.ToInt32(buf[1]),
                    Convert.ToInt32(buf[0]));
                
                allInfoMoneys = allInfoMoneys.Where(x => x.Date.Date <= date);
            }
            
            if (query.ShopId != 0)
                allInfoMoneys = allInfoMoneys.Where(x => x.Sale.ShopId == query.ShopId);

            if (query.Score != 0)
                allInfoMoneys = allInfoMoneys.Where(x => x.MoneyWorkerId == query.Score);

            var infoMoneysList = allInfoMoneys
                .Select(x => new MoneyHistoryVM()
                {
                    Id = x.Id,
                    Sum = x.Sum,
                    Date = x.Date.ToString("dd.MM.yyyy"),
                    Comment = x.Comment,
                    PaymentType = x.PaymentType,
                    Sale = x.Sale,
                    MoneyWorker = x.MoneyWorker,
                    MoneyOperationType = x.MoneyOperationType,
                    ShopTitle = x.Sale.Shop.Title
                }).ToList();

            if (query.Type != 0)
                infoMoneysList = infoMoneysList
                    .Where(x => (int) x.MoneyOperationType == query.Type).ToList();

            return infoMoneysList
                .Select(x => new MoneyHistoryVM()
                {
                    Id = x.Id,
                    Sum = x.Sum,
                    Date = x.Date,
                    Comment = repaidDebtsIds.Contains(x.Id)
                        ? repaidDebts.FirstOrDefault(r
                            => r.Item1 == x.Id).Item2
                        : x.Comment,
                    PaymentType = x.PaymentType,
                    Sale = x.Sale,
                    MoneyWorker = x.MoneyWorker,
                    MoneyOperationType = x.MoneyOperationType,
                    ShopTitle = x.ShopTitle
                })
                .OrderByDescending(x => x.Id)
                .ToList();
        }

        private static ICollection<(int, string)> DebtRepaymentOperations(
            PostgresContext postgresContext,
            ShopContext shopContext)
        {
            var repaidDebts = postgresContext.RepaidDebtsOld.ToList()
                .Join(shopContext.Suppliers,
                    repaid => repaid.SupplierId,
                    supplier => supplier.Id,
                    (repaid, supplier) => (repaid.InfoMoneyId, supplier.Title))
                .ToList();

            return repaidDebts;
        }
    }

    public class MoneyHistoryFilterQuery
    {
        public string StartDate { get; private set; }
        public string EndDate { get; private set; }
        public int ShopId { get; private set; }
        public int Score { get; private set; }
        public int Type { get; private set; }

        public MoneyHistoryFilterQuery(string startDate, string endDate, int shopId, int score, int type)
            => (StartDate, EndDate, ShopId, Score, Type) = (startDate, endDate, shopId, score, type);
    }
}