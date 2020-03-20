using System.Linq;
using Base.Services.Abstract;
using Data.Entities;
using Data.Enums;
using Data.Services.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Data.Services.Concrete
{
    public class SaleInfoService : ISaleInfoService
    {
        private readonly IBaseObjectService<SaleProduct> _saleProductService;
        private readonly IBaseObjectService<Partner> _parnterService;
        private readonly ISaleService _saleService;
        private readonly IInfoMoneyService _infoMoneyService;

        public SaleInfoService(IBaseObjectService<SaleProduct> saleProductService,
            IBaseObjectService<Partner> partnerService,
            ISaleService saleService,
            IInfoMoneyService infoMoneyService)
        {
            _saleProductService = saleProductService;
            _parnterService = partnerService;
            _saleService = saleService;
            _infoMoneyService = infoMoneyService;
        }

        public bool HasAdditionalProduct(int id)
        {
            return _saleProductService.All().Any(x => x.SaleId == id && x.Additional);
        }

        public string BuyerTitle(int id)
        {
            var sale = _saleService.Get(id);
            var res = _parnterService.All().FirstOrDefault(x => x.Id == sale.PartnerId)?.Title
                      ?? "Обычный покупатель";
            
            return res;
        }

        public string FirstProductTitle(int id)
        {
            return _saleProductService.All()
                .Include(x => x.Product).FirstOrDefault(x => x.SaleId == id)?.Product?.Title ?? "";
        }

        public IQueryable<SaleProduct> GetProductsBySaleId(int saleId)
        {
            return _saleProductService.All().Where(x => x.SaleId == saleId);
        }

        public PaymentType PaymentType(int id)
        {
            var infoMonies = _infoMoneyService.All().Where(x => x.SaleId == id);

            bool cash = infoMonies.Any(x => x.PaymentType == Enums.PaymentType.Cash);
            bool cashless = infoMonies.Any(x => x.PaymentType == Enums.PaymentType.Cashless);
            bool mixed = cash && cashless;

            if (mixed)
                return Enums.PaymentType.Mixed;
            if (cashless)
                return Enums.PaymentType.Cashless;
            return Enums.PaymentType.Cash;
        }
    }
}