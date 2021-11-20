using System.Linq;
using Base.Services.Abstract;
using Data.Entities;
using Data.ViewModels;

namespace Data.Services.Abstract
{
    public interface IInfoMoneyService : IBaseObjectService<InfoMoney>
    {
        decimal GetMoneyWorkerBalance(ShopContext db, int id);

        decimal Balance();

        decimal SaleSum(int id);
        
    }
}