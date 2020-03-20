using System;
using Base.Services.Abstract;
using Data.Entities;

namespace Data.Services.Abstract
{
    public interface IShopService : IBaseObjectService<Shop>
    {
        decimal Turnover(int id);

        decimal CashOnHand(int id);

        decimal Margin(int id);

        decimal DateTurnover(int id, DateTime dateStart, DateTime dateEnd);

        decimal DateMargin(int id, DateTime dateStart, DateTime dateEnd);

        decimal DateAverageMargin(int id, DateTime dateStart, DateTime dateEnd);

        decimal DateAverageCheck(int id, DateTime dateStart, DateTime dateEnd);

        Shop ShopByUserId(int id);
    }
}