using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Services.Abstract
{
    public interface IMarginService
    {
        decimal GetMarginShop(int shopId, int categoryId);

        decimal GetClearMarginShop(int shopId, int categoryId);

        decimal GetMarginPartners(int categoryId);

        decimal GetMarginRussian(int categoryId);
    }
}
