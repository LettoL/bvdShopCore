using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Services.Abstract
{
    public interface IPrimeCostService
    {
        decimal GetPrimeCostShop(int shopId, int categoryId);
        decimal GetClearPrimeCostShop(int shopId, int categoryId);
        decimal GetPrimeCostPartners(int categoryId);
        decimal GetPrimeCostRussian(int categoryId);
    }
}
