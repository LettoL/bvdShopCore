using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Services.Abstract
{
    public interface ITurnOverService
    {
        decimal GetTurnOverShop(int shopId, int categoryId);
        decimal GetClearTurnOverShop(int shopId, int categoryId);
        decimal GetTurnOverPartners(int categoryId);
        decimal GetTurnOverRussian(int categoryId);
    }
}
