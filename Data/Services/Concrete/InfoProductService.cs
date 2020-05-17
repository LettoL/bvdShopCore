using System;
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
          DateTime? periodStart = null;
          DateTime? periodEnd = null;
          
          //Console.WriteLine("PeriodStart1: " + model.periodStart.Value.ToString());
          //Console.WriteLine("PeriodEnd1: " + model.periodEnd.Value.ToString());

          if (model.periodStart != null)
          {
            var buf = model.periodStart.Split('.');
            
            periodStart = new DateTime(
              Convert.ToInt32(buf[2]),
              Convert.ToInt32(buf[1]),
              Convert.ToInt32(buf[0]));
          }

          if (model.periodEnd != null)
          {
            var buf = model.periodEnd.Split('.');
            
            periodEnd = new DateTime(
              Convert.ToInt32(buf[2]),
              Convert.ToInt32(buf[1]),
              Convert.ToInt32(buf[0]));
          }
          
          Console.WriteLine("PeriodStart2: " + periodStart.ToString());
          Console.WriteLine("PeriodEnd2: " + periodEnd.ToString());
          
          var query = All()
            //.Where(x => _dataCompareService.IsPeriod(x.Date.Date, model.periodStart, model.periodEnd))
            .Where(x => periodStart == null
                        || x.Date.Date >= periodStart)
            .Where(x => periodEnd == null
                        || x.Date.Date <= periodEnd)
            .Where(x => model.shopId == 0 || x.ShopId == model.shopId);

            return query;
        }
    }
}