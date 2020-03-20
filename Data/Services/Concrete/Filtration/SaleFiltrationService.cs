using System;
using Data.Entities;
using System.Linq;
using Data.FiltrationModels;

namespace Data.Services.Concrete.Filtration
{
    public static class SaleFiltrationService
    {
        public static IQueryable<Sale> Filtration(this IQueryable<Sale> query,
            SaleFiltrationModel model)
        {
            query = query.IsShopId(model.shopId)
                .IsType(model.type)
                .IsBuyer(model.buyer)
                .IsPeriod(model.periodStart, model.periodEnd)
                .IsForRF(model.forRF);
            
            return query;
        }

        private static IQueryable<Sale> IsShopId(this IQueryable<Sale> query, int shopId)
        {
            if (shopId != 0)
                query = query.Where(x => x.ShopId == shopId);

            return query;
        }
        
        private static IQueryable<Sale> IsType(this IQueryable<Sale> query, int type)
        {
            if (type != 0)
                query = query.Where(x => x.SalesProducts.Any(sp => sp.Additional));

            return query;
        }

        private static IQueryable<Sale> IsBuyer(this IQueryable<Sale> query, int buyerId)
        {
            if (buyerId > 0) //Конкретный партнёр
                query = query.Where(x => x.PartnerId == buyerId);
            if (buyerId == -1) //Все продажи партнерам
                query = query.Where(x => x.PartnerId != null);

            return query; //По умолчанию возвращаем все продажи
        }

        private static IQueryable<Sale> IsPeriod(this IQueryable<Sale> query,
            DateTime? periodStart, DateTime? periodEnd)
        {
            if (periodStart != null)
                query = query.Where(x => x.Date.Date >= periodStart);

            if (periodEnd != null)
                query = query.Where(x => x.Date.Date <= periodEnd);

            return query;
        }

        private static IQueryable<Sale> IsForRF(this IQueryable<Sale> query, bool forRF)
        {
            if (forRF == true)
                query = query.Where(x => x.ForRussian == true);

            return query;
        }
    }
}