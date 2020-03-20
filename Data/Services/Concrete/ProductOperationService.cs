using System;
using System.Linq;
using Base.Services.Abstract;
using Data.Entities;
using Data.Enums;
using Data.Services.Abstract;
using Data.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Data.Services.Concrete
{
    public class ProductOperationService : IProductOperationService
    {
        private readonly IProductService _productService;
        private readonly IBaseObjectService<SupplyHistory> _supplyHistoryService;
        private readonly IBaseObjectService<SupplyProduct> _supplyProductService;
        private readonly IBaseObjectService<DeferredSupplyProduct> _deferredSupplyProductService;
        private readonly IBaseObjectService<Supplier> _supplierService;
        private readonly IInfoProductService _infoProductService;
        private readonly IBaseObjectService<ProductInformation> _productInformationService;

        public ProductOperationService(IProductService productService,
            IBaseObjectService<SupplyHistory> supplyHistory,
            IBaseObjectService<SupplyProduct> supplyProductService,
            IBaseObjectService<DeferredSupplyProduct> deferredSupplyProductService,
            IInfoProductService infoProductService,
            IBaseObjectService<Supplier> supplierService,
            IBaseObjectService<ProductInformation> productInformationService)
        {
            _productService = productService;
            _supplyHistoryService = supplyHistory;
            _supplyProductService = supplyProductService;
            _deferredSupplyProductService = deferredSupplyProductService;
            _infoProductService = infoProductService;
            _supplierService = supplierService;
            _productInformationService = productInformationService;
        }

        public void Supply(SupplyProductVM obj)
        {
            var product = _productService.All().FirstOrDefault(x => x.Title == obj.Name && x.ShopId == obj.ShopId);

            if (product == null)
            {
                var productForCopy = _productService.All().FirstOrDefault(x => x.Id == obj.ProductId);

                product = _productService.Create(new Product()
                {
                    Title = productForCopy.Title,
                    Code = productForCopy.Code,
                    Reserv = productForCopy.Reserv,
                    BookingAmount = 0,
                    Cost = productForCopy.Cost,
                    CategoryId = productForCopy.CategoryId,
                    ShopId = obj.ShopId,
                });

            }

            var supplyHistory = _supplyHistoryService.Create(new SupplyHistory());

            InfoProduct info;

            if (obj.SupplierId != 0)
            {
                info = new InfoProduct()
                {
                    Amount = obj.Amount,
                    Date = DateTime.Now,
                    ProductId = product.Id,
                    ShopId = product.ShopId,
                    SupplierId = obj.SupplierId,
                    Type = InfoProductType.Supply,
                    SupplyHistoryId = supplyHistory.Id
                };

                var supplyProduct = new SupplyProduct()
                {
                    ProductId = product.Id,
                    SupplierId = obj.SupplierId,
                    TotalAmount = obj.Amount,
                    RealizationAmount = obj.Realization == SupplyType.ForRealization ? obj.Amount : 0,
                    AdditionalCost = obj.AdditionalCost / obj.Amount,
                    ProcurementCost = obj.ProcurementCost / obj.Amount,
                    FinalCost = (obj.ProcurementCost / obj.Amount) + (obj.AdditionalCost / obj.Amount),
                    StockAmount = obj.Amount,
                    SupplyHistoryId = supplyHistory.Id
                };

                _supplyProductService.Create(supplyProduct);

                if (obj.Realization == SupplyType.DeferredPayment)
                {
                    _deferredSupplyProductService.Create(new DeferredSupplyProduct()
                    {
                        Date = obj.Date,
                        SupplyProductId = supplyProduct.Id
                    });
                }

            }
            else
            {             
                var supply = _supplyProductService.Create(new SupplyProduct()
                {
                    ProductId = product.Id,
                    TotalAmount = obj.Amount,
                    RealizationAmount = obj.Realization == SupplyType.ForRealization ? obj.Amount : 0,
                    AdditionalCost = obj.AdditionalCost / obj.Amount,
                    ProcurementCost = obj.ProcurementCost / obj.Amount,
                    FinalCost = (obj.ProcurementCost / obj.Amount) + (obj.AdditionalCost / obj.Amount),
                    StockAmount = obj.Amount,
                    SupplyHistoryId = supplyHistory.Id
                });

                info = new InfoProduct()
                {
                    Amount = obj.Amount,
                    Date = DateTime.Now,
                    ProductId = product.Id,
                    ShopId = product.ShopId,
                    Type = InfoProductType.Supply,
                    SupplyHistoryId = supplyHistory.Id,
                };
            }


            _infoProductService.Create(info);
        }

        public void WriteOff(int productId, int supplyId, int amount)
        {
            var product = _productService.Get(productId);
            var supply = _supplyProductService.All().FirstOrDefault(x => x.Id == supplyId);

            supply = SupplyProductWriteOff(supply, amount);

            CreateHistory(new InfoProduct
            {
                Amount = amount,
                ProductId = product.Id,
                Date = DateTime.Now,
                ShopId = product.ShopId,
                Type = InfoProductType.Writeoff,
                SupplyProductId = supply.Id
            });

            _supplyProductService.Update(supply);
        }

        public decimal RealizationProduct(int productId, int amount, int saleId)
        {
            decimal cost = 0;

            if (amount <= 0)
                return cost;

            var supplyProductRealization = _supplyProductService.All()
                .FirstOrDefault(x => x.ProductId == productId && x.RealizationAmount > 0);

            if (supplyProductRealization != null)//Есть поставки с товаром под реализацию
            {
                if (supplyProductRealization.RealizationAmount >= amount)
                {
                    IncreaseDebtToSupplier((int) supplyProductRealization.SupplierId,
                        supplyProductRealization.ProcurementCost * amount);

                    DecreaseRealizationAmountOfSupplyProduct(supplyProductRealization.Id, amount);
                    
                    cost += PrimeCostCalculate(supplyProductRealization.FinalCost, amount);

                    _productInformationService.Create(new ProductInformation()
                    {
                        SupplyProductId = supplyProductRealization.Id,
                        Amount = amount,
                        ForRealization = true,
                        SaleId = saleId,
                        FinalCost = supplyProductRealization.FinalCost * amount,
                        ProductId = productId
                    });
                }
                else
                {
                    var buf = amount - supplyProductRealization.RealizationAmount;
                    var bufAmount = supplyProductRealization.RealizationAmount;

                    IncreaseDebtToSupplier((int) supplyProductRealization.SupplierId,
                        supplyProductRealization.ProcurementCost * bufAmount);

                    DecreaseRealizationAmountOfSupplyProduct(supplyProductRealization.Id, bufAmount);
                    
                    cost += PrimeCostCalculate(supplyProductRealization.FinalCost, bufAmount);

                    _productInformationService.Create(new ProductInformation()
                    {
                        SupplyProductId = supplyProductRealization.Id,
                        Amount = bufAmount,
                        ForRealization = true,
                        SaleId = saleId,
                        FinalCost = supplyProductRealization.FinalCost * bufAmount,
                        ProductId = productId
                    });

                    cost += RealizationProduct(productId, buf, saleId);
                }
            }
            else //Нет поставок под реализацию
            {
                var supplyProduct = _supplyProductService.All()
                    .FirstOrDefault(x => x.ProductId == productId && x.StockAmount > 0);

                if (supplyProduct != null)
                {
                    if (supplyProduct.StockAmount >= amount)
                    {
                        DecreaseAmountOfSupplyProduct(supplyProduct.Id, amount);
                        
                        cost += PrimeCostCalculate(supplyProduct.FinalCost, amount);

                        _productInformationService.Create(new ProductInformation()
                        {
                            SupplyProductId = supplyProduct.Id,
                            Amount = amount,
                            ForRealization = false,
                            SaleId = saleId,
                            FinalCost = supplyProduct.FinalCost * amount,
                            ProductId = productId
                        });
                    }
                    else
                    {
                        var buf = amount - supplyProduct.StockAmount;
                        var bufAmount = supplyProduct.StockAmount;

                        DecreaseAmountOfSupplyProduct(supplyProduct.Id, bufAmount);
                        
                        cost += PrimeCostCalculate(supplyProduct.FinalCost, bufAmount);

                        _productInformationService.Create(new ProductInformation()
                        {
                            SupplyProductId = supplyProduct.Id,
                            Amount = bufAmount,
                            ForRealization = false,
                            SaleId = saleId,
                            FinalCost = supplyProduct.FinalCost * bufAmount,
                            ProductId = productId
                        });

                        cost += RealizationProduct(productId, buf, saleId);
                    }
                }
            }

            return cost;
        }

        public void ChangePrice(int productId, decimal price)
        {
            var product = _productService.Get(productId);

            product.Cost = price;

            _productService.Update(product);
        }    

        private SupplyProduct SupplyProductWriteOff(SupplyProduct obj, int amount)
        {
            obj.StockAmount -= amount;

            if (obj.RealizationAmount > amount)
                obj.RealizationAmount -= amount;
            else
                obj.RealizationAmount = 0;

            obj.TotalAmount -= amount;

            return obj;
        }

        private void CreateHistory(InfoProduct obj)
        {
            _infoProductService.Create(obj);
        }

        private void IncreaseDebtToSupplier(int supplierId, decimal sum)
        {
            var supplier = _supplierService.Get(supplierId);

            supplier.Debt += sum;

            _supplierService.Update(supplier);
        }

        private void DecreaseRealizationAmountOfSupplyProduct(int supplyProductId, int amount)
        {
            var supplyProduct = _supplyProductService.Get(supplyProductId);

            supplyProduct.RealizationAmount -= amount;
            
            DecreaseAmountOfSupplyProduct(supplyProductId, amount);
        }

        private void DecreaseAmountOfSupplyProduct(int supplyProductId, int amount)
        {
            var supplyProduct = _supplyProductService.Get(supplyProductId);

            supplyProduct.StockAmount -= amount;

            _supplyProductService.Update(supplyProduct);
        }

        private decimal PrimeCostCalculate(decimal cost, int amount)
        {
            return cost * amount;
        }
    }
}