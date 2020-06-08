using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Entities;
using Data.Enums;
using Handlers.CommandHandlers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostgresData;
using WebUI.Commands;
using SupplyProduct = Data.Entities.SupplyProduct;

namespace WebUI.API
{
    [ApiController]
    [Route("/api/supplyProducts")]
    public class SupplyProducts : ControllerBase
    {
        private readonly ShopContext _db;
        private readonly PostgresContext _postgresContext;

        public SupplyProducts(ShopContext db, PostgresContext postgresContext)
        {
            _db = db;
            _postgresContext = postgresContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _db.SupplyProducts
                .Where(x => x.SupplyHistoryId > 0)
                .Select(s => new Dtos.SupplyProductItem()
                {
                    SupplyProductId = s.Id,
                    //DateTime = i.Date,
                    ProductId = s.ProductId,
                    ProductTitle = s.Product.Title,
                    ShopAmount = s.StockAmount,
                    SuppliedAmount = s.TotalAmount,
                    ShopId = s.Product.ShopId,
                    ShopTitle = s.Product.Shop.Title,
                    SupplierId = s.SupplierId ?? 0,
                    SupplierName = s.Supplier.Title,
                    ProcurementCost = s.ProcurementCost
                })               
                /*.Join(_db.InfoProducts.Where(x => x.SupplyHistoryId > 0),
                    s => s.SupplyHistoryId,
                    i => i.SupplyHistoryId,
                    (s, i) => new Dtos.SupplyProductItem()
                    {
                        SupplyProductId = s.Id,
                        DateTime = i.Date,
                        ProductId = s.ProductId,
                        ProductTitle = s.Product.Title,
                        ShopAmount = s.StockAmount,
                        SuppliedAmount = s.TotalAmount,
                        ShopId = s.Product.ShopId,
                        ShopTitle = s.Product.Shop.Title,
                        SupplierId = s.SupplierId ?? 0,
                        SupplierName = s.Supplier.Title
                    })*/
                .OrderByDescending(x => x.SupplyProductId)
                .ToListAsync();
            
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Commands.SupplyProduct command)
        {
            decimal procurementCost = 0;
            if (!Decimal.TryParse(command.ProcurementCost, out procurementCost))
                return BadRequest("Неверна введена закупочная цена");
            
            var supplyProduct = new Handlers.Commands.SupplyProduct()
            {
                ProductId = command.ProductId,
                Amount = command.Amount,
                ShopId = command.ShopId,
                SupplierId = command.SupplierId,
                Type = command.Type,
                ProcurementCost = procurementCost
            };
            
            var result = await SupplyProductHandler.Handle(supplyProduct, _postgresContext);
            
            return Ok(result);
        }

        [HttpPost]
        [Route("import")]
        public async Task<IActionResult> Import([FromBody] ImportProductsForm command)
        {
            try
            {
                var result = command;

                var shopId = command.Shop;
                var categoryId = command.Category;
                var supplierId = command.Supplier;
                var realization = command.SupplyType;
                var additionalCost = 0;
                var date = DateTime.Now;
                
                //old
                var supplyHistory = _db.SupplyHistories
                    .Add(new SupplyHistory());

                foreach (var product in command.Products)
                {
                    decimal procurementCost = 0;
                    decimal a;
                    if (Decimal.TryParse(product.Price.Replace(',', '.'), out a))
                        procurementCost = a;
                    else
                        throw new Exception("Закупочная стоимость неверного формата в товаре: " 
                                            + product.Title);
                  
                    var existingProduct = _db.Products
                        .FirstOrDefault(x => x.Title == product.Title
                                                  && x.ShopId == shopId);

                    if (existingProduct == null)
                    {
                        var productCost = _db.Products
                            .FirstOrDefault(x => x.Title == product.Title)?.Cost ?? 0;
                        
                        var createProduct = _db.Products
                          .Add(new Product()
                          {
                              Code = product.Code,
                              Title = product.Title,
                              ShopId = shopId,
                              CategoryId = categoryId,
                              Cost = productCost
                          });

                        _db.InfoProducts.Add(new InfoProduct()
                        {
                            Amount = product.Amount,
                            Date = DateTime.Now.AddHours(3),
                            Product = createProduct.Entity,
                            SupplierId = supplierId,
                            Type = InfoProductType.Supply,
                            ShopId = shopId,
                            SupplyHistory = supplyHistory.Entity
                        });

                        var supplyProduct = _db.SupplyProducts
                            .Add(new SupplyProduct()
                            {
                                Product = createProduct.Entity,
                                SupplierId = supplierId,
                                RealizationAmount = (SupplyType) realization == SupplyType.ForRealization
                                    ? product.Amount
                                    : 0,
                                TotalAmount = product.Amount,
                                AdditionalCost = 0,
                                ProcurementCost = procurementCost,
                                FinalCost = procurementCost,
                                StockAmount = product.Amount,
                                SupplyHistory = supplyHistory.Entity
                            });

                        if ((SupplyType) realization == SupplyType.DeferredPayment)
                            _db.DeferredSupplyProducts.Add(new DeferredSupplyProduct()
                            {
                                Date = date,
                                SupplyProduct = supplyProduct.Entity
                            });
                    }
                    else
                    {
                        var supplyProduct = _db.SupplyProducts
                            .Add(new SupplyProduct()
                            {
                                ProductId = existingProduct.Id,
                                RealizationAmount = (SupplyType) realization == SupplyType.ForRealization
                                    ? product.Amount
                                    : 0,
                                TotalAmount = product.Amount,
                                AdditionalCost = 0,
                                ProcurementCost = procurementCost,
                                FinalCost = procurementCost,
                                StockAmount = product.Amount,
                                SupplyHistory = supplyHistory.Entity,
                                SupplierId = supplierId
                            });

                        _db.InfoProducts.Add(new InfoProduct()
                        {
                            Amount = product.Amount,
                            Date = DateTime.Now.AddHours(3),
                            Product = existingProduct,
                            SupplierId = supplierId,
                            Type = InfoProductType.Supply,
                            ShopId = shopId,
                            SupplyHistory = supplyHistory.Entity
                        });

                        if ((SupplyType) realization == SupplyType.DeferredPayment)
                            _db.DeferredSupplyProducts.Add(new DeferredSupplyProduct()
                            {
                                Date = date,
                                SupplyProduct = supplyProduct.Entity
                            });
                    }
                }

                _db.SaveChanges();

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}