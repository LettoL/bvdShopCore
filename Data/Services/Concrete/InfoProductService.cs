using System.Linq;
using Base.Services.Abstract;
using Base.Services.Concrete;
using Data.Entities;
using Data.FiltrationModels;
using Data.Services.Abstract;

namespace Data.Services.Concrete
{
    public class InfoProductService : BaseObjectService<InfoProduct>, IInfoProductService
    {
        private readonly IDataCompareService _dataCompareService;

        public InfoProductService(ShopContext context,
            IDataCompareService dataCompareService) : base(context)
        {
            _dataCompareService = dataCompareService;
        }

        public IQueryable<InfoProduct> Filtration(InfoProductFiltrationModel model)
        {
            var query = All()
                .Where(x => _dataCompareService.IsPeriod(x.Date.Date, model.periodStart, model.periodEnd))
                .Where(x => model.shopId == 0 || x.ShopId == model.shopId)
                .Where(x => model.type == 0 || (int)x.Type == model.type);

            return query;
        }
    }
}