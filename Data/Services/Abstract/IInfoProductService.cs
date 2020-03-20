using Base.Services.Abstract;
using Data.Entities;
using System.Linq;
using Data.FiltrationModels;

namespace Data.Services.Abstract
{
    public interface IInfoProductService : IBaseObjectService<InfoProduct>
    {
        IQueryable<InfoProduct> Filtration(InfoProductFiltrationModel model);
    }
}