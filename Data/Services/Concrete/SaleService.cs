using System;
using System.Collections.Generic;
using System.Linq;
using Base.Services.Abstract;
using Base.Services.Concrete;
using Data.Entities;
using Data.Enums;
using Data.FiltrationModels;
using Data.Services.Abstract;
using Data.Services.Concrete.Filtration;
using Data.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Data.Services.Concrete
{
    public class SaleService : BaseObjectService<Sale>, ISaleService
    {
        private readonly IInfoMoneyService _infoMoneyService;
        private readonly IBaseObjectService<SaleProduct> _saleProductService;
        private readonly IProductService _productService;
        private readonly IShopService _shopService;
        private readonly IBaseObjectService<SaleInformation> _saleInformationService;
        private readonly IBaseObjectService<MoneyWorker> _moneyWorkerService;
        private readonly IMoneyOperationService _moneyOperationService;
        private readonly IProductOperationService _productOperationService;
        private readonly IBaseObjectService<ProductInformation> _productInformationService;

        public SaleService(ShopContext context,
            IInfoMoneyService infoMoneyService,
            IBaseObjectService<SaleProduct> saleProductService,
            IProductService productService,
            IShopService shopService,
            IBaseObjectService<SaleInformation> saleInformationService,
            IBaseObjectService<MoneyWorker> moneyWorkerService,
            IMoneyOperationService moneyOperationService,
            IBaseObjectService<ProductInformation> productInformationService,
            IProductOperationService productOperationService) : base(context)
        {
            _infoMoneyService = infoMoneyService;
            _saleProductService = saleProductService;
            _productService = productService;
            _shopService = shopService;
            _saleInformationService = saleInformationService;
            _moneyWorkerService = moneyWorkerService;
            _moneyOperationService = moneyOperationService;
            _productOperationService = productOperationService;
            _productInformationService = productInformationService;
        }

        public bool ForTest(int id)
        {
            return id == 1;
        }

        public Sale Create(ShopContext db, SaleCreateVM obj, int userId)
        {
            var shop = _shopService.ShopByUserId(db, userId);
                
            if (shop == null)
                throw new Exception("Текущий пользователь не привязан к магазину");

            if (obj.CashlessSum > 0 && obj.MoneyWorkerId == null)
            {
                throw new Exception("Вы не выбрали счет для безналичного перевода, проведите продажу заново");
            }

            var sum = obj.CashSum + obj.CashlessSum;

            var sale = db.Sales.Add(new Sale()
            {
                Date = DateTime.Now.AddHours(3),
                AdditionalComment = obj.AdditionalComment,
                Comment = obj.Comment,
                ShopId = shop.Id,
                PartnerId = obj.PartnerId,
                Sum = sum,
                CashSum = obj.CashSum,
                CashlessSum = obj.CashlessSum,
                Discount = obj.Discount,
                Payment = obj.Payment,
                SaleType = obj.Payment != true ? SaleType.DefferedSale : SaleType.Sale,
                ForRussian = obj.ForRussian
            }).Entity;

            db.SaveChanges();

            if (obj.Payment)
                _moneyOperationService.SalePayment(db, obj.CashSum, obj.CashlessSum,
                    sale, shop.Id, obj.MoneyWorkerId);
            else
                db.SaleInformations.Add(new SaleInformation()
                {
                    MoneyWorkerForIncomeId = shop.Id == 0 ? null : obj.MoneyWorkerId,
                    SaleId = sale.Id,
                    SaleType = SaleType.DefferedSale
                });

            decimal primeCost = 0;

            foreach (var product in obj.Products)
            {
                db.SalesProducts.Add(new SaleProduct()
                {
                    Amount = product.Amount,
                    ProductId = product.Id,
                    SaleId = sale.Id,
                    Additional = product.Additional,
                    Cost = product.Cost
                });

                primeCost += _productOperationService.RealizationProduct(db, product.Id, product.Amount, sale.Id);
            }
            
            sale.PrimeCost = primeCost;
            sale.Margin = sale.Sum - sale.PrimeCost;

            db.SaleInformations.Add(new SaleInformation
            {
                SaleId = sale.Id,
                SaleType = SaleType.Sale
            });

            db.Entry(sale).State = EntityState.Modified;
            db.SaveChanges();
            
            return sale;
        }

        public Sale CreatePostPayment(ShopContext db, SaleCreateVM obj, int userId)
        {
            var shop = _shopService.ShopByUserId(db, userId);

            if (shop == null)
                throw new Exception("Текущий пользователь не привязан к магазину");

            var productsId = obj.Products.Select(x => x.Id);
            var products = new List<Product>();

            foreach(var id in productsId)
            {
                products.Add(_productService.All().FirstOrDefault(x => x.Id == id));
            }

            var sum = obj.Sum;

            var sale = db.Sales.Add(new Sale()
            {
                Date = DateTime.Now.AddHours(3),
                ShopId = shop.Id,
                AdditionalComment = obj.AdditionalComment,
                PartnerId = obj.PartnerId,
                Sum = sum,
                CashSum = obj.CashSum,
                CashlessSum = obj.CashlessSum,
                Discount = obj.Discount,
                Payment = false,
                Comment = obj.Comment,
                PrimeCost = 0, // Вносим при закрытии продажи
                Margin = 0, // Так же просчитываем при закрытии продажи
                SaleType = obj.SaleType,
                ForRussian = obj.ForRussian
            }).Entity;

            foreach (var product in obj.Products)
            {
                db.SalesProducts.Add(new SaleProduct()
                {
                    Amount = product.Amount,
                    ProductId = product.Id,
                    Sale = sale,
                    Additional = product.Additional,
                    Cost = product.Cost
                });
            }

            return sale;
        }

        public void ChangeProductProcurementCost(int productId, int saleId, decimal procurementCost)
        {
            var productInformations = _productInformationService
                .All()
                .Where(x => x.SaleId == saleId && x.ProductId == productId)
                .Include(x => x.SupplyProduct)
                .ToList();

            var sale = base.All().FirstOrDefault(x => x.Id == saleId);

            foreach (var productInformation in productInformations)
            {
                decimal FinalCostDifference = 0;

                if (productInformation.SupplyProduct != null)
                {
                   FinalCostDifference += (productInformation.SupplyProduct.AdditionalCost + procurementCost) *
                        productInformation.Amount - productInformation.FinalCost;

                    productInformation.FinalCost = productInformation.Amount *
                        (productInformation.SupplyProduct.AdditionalCost + procurementCost);

                    _productInformationService.Update(productInformation);
                }
                else
                {
                    FinalCostDifference += (productInformation.AdditionalCost + procurementCost) *
                        productInformation.Amount - productInformation.FinalCost;

                    productInformation.FinalCost = productInformation.Amount *
                        (productInformation.AdditionalCost + procurementCost);

                    _productInformationService.Update(productInformation);
                }

                sale.PrimeCost += FinalCostDifference;
                sale.Margin -= FinalCostDifference;
            }                    

            base.Update(sale);
        }

        public IQueryable<Sale> DeferredSalesFromStock(ShopContext context)
        {
            return context.Sales
                .Where(s => s.Payment == false &&
                            context.SaleInformations
                                .Count(x => x.SaleId == s.Id
                                            && x.SaleType == SaleType.DefferedSaleFromStock) > 0 &&
                            s.Sum <= _infoMoneyService.All().Where(x => x.SaleId == s.Id).Sum(x => x.Sum));
        }

        public IQueryable<Sale> SalesWithOpenPayments(ShopContext context)
        {
            return context.Sales.Where(s => s.Payment == false &&
                                         context.SaleInformations
                                             .Count(x => x.SaleId == s.Id &&
                                                         x.SaleType == SaleType.SaleFromStock) > 0);
        }

        public string ProductTitle(int id)
        {
            return _saleProductService.All().Include(x => x.Product)
                       .FirstOrDefault(x => x.SaleId == id)?.Product?.Title ?? "";
        }

        public void ClosePostPayment(int id, int moneyWorkerType,
            int moneyWorkerId, int moneyWorkerCashlessId, decimal totalCost)
        {
            var sale = Get(id);

            var moneyWorker = _moneyWorkerService.All().FirstOrDefault(x => x.Id == moneyWorkerId);

            decimal paymentsCash = _infoMoneyService.All().Where(x => x.SaleId == sale.Id && x.PaymentType == PaymentType.Cash).Sum(x => x.Sum);
            decimal paymentsCashless = _infoMoneyService.All().Where(x => x.SaleId == sale.Id && x.PaymentType == PaymentType.Cashless).Sum(x => x.Sum);

            if (sale.CashSum > 0)
            {
                var newCashMoney = _infoMoneyService.Create(new InfoMoney()
                {
                    MoneyOperationType = MoneyOperationType.Sale,
                    MoneyWorkerId = moneyWorker.Id,
                    PaymentType = PaymentType.Cash, 
                    SaleId = sale.Id,
                    Sum = sale.CashSum - paymentsCash  
                });

                paymentsCash += newCashMoney.Sum;
            }
            if(sale.CashlessSum > 0)
            {
                var newCashlessMoney = _infoMoneyService.Create(new InfoMoney()
                {
                    MoneyOperationType = MoneyOperationType.Sale,
                    MoneyWorkerId = moneyWorkerCashlessId,
                    PaymentType = PaymentType.Cashless,
                    SaleId = sale.Id,
                    Sum = sale.CashlessSum - paymentsCashless
                });

                paymentsCashless += newCashlessMoney.Sum;
            }
            

            sale.Margin = sale.Sum - totalCost;
            sale.PrimeCost = totalCost;
            sale.Sum = paymentsCash + paymentsCashless;

            sale.Payment = true;

            var saleInformation = _saleInformationService.All().FirstOrDefault(x => x.SaleId == sale.Id);
            _saleInformationService.Delete(saleInformation);

            Update(sale);
        }

        public IQueryable<Sale> Filtration(SaleFiltrationModel model)
        {
            var query = SaleFiltrationService.Filtration(All().Where(x => x.Payment), model);

            return query;
        }

        public IQueryable<Sale> GetSalesByShop(int id)
        {
            var query = this.All()
                .Where(x => x.ShopId == id && x.Payment == true);

            return query;
        }

        public IQueryable<Sale> GetSalesByPartners()
        {
            var query = this.All()
                .Where(x => x.PartnerId != null && x.Payment == true);

            return query;
        }

        public IQueryable<Sale> GetSalesByRussian()
        {
            var query = this.All()
                .Where(x => x.ForRussian == true && x.Payment == true);

            return query;
        }
    }
}
