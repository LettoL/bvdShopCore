using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Services.Abstract
{
    public interface ISalesAmountService
    {
        int GetSalesAmountShop(int shopId, int categoryId);
        int GetClearSalesAmountShop(int shopId, int categoryId);
        int GetSalesAmountPartners(int categoryId);
        int GetSalesAmountRussian(int categoryId);
    }
}
