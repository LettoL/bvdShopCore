using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Entities;
using Data.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebUI.Commands;

namespace WebUI.API
{
    [ApiController]
    [Route("/api/supplyProducts")]
    public class SupplyProducts : ControllerBase
    {
        private readonly ShopContext _db;

        public SupplyProducts(ShopContext db)
        {
            _db = db;
        }

        [HttpPost]
        [Route("import")]
        public async Task<IActionResult> Import([FromBody] ImportProducts command)
        {
            try
            {
                var result = command;

                var shopId = 1;
                var categoryId = 1;
                var supplierId = 1;
                var realization = 1;
                var additionalCost = 0;
                var date = DateTime.Now;
                
                //old
                var supplyHistory = await _db.SupplyHistories
                    .AddAsync(new SupplyHistory());

                foreach (var product in command.Products)
                {
                    var existingProduct = await _db.Products
                        .FirstOrDefaultAsync(x => x.Title == product.Title
                                                  && x.ShopId == shopId);

                    if (existingProduct == null)
                    {
                        var productCost = _db.Products
                            .FirstOrDefault(x => x.Title == product.Title)?.Cost ?? 0;
                        
                        var createProduct = await _db.Products.AddAsync(new Product()
                        {
                            Code = product.Code,
                            Title = product.Title,
                            ShopId = shopId,
                            CategoryId = categoryId,
                            Cost = productCost
                        });

                        await _db.InfoProducts.AddAsync(new InfoProduct()
                        {
                            Amount = product.Amount,
                            Date = DateTime.Now.AddHours(3),
                            Product = createProduct.Entity,
                            SupplierId = supplierId,
                            Type = InfoProductType.Supply,
                            ShopId = shopId,
                            SupplyHistoryId = supplyHistory.Entity.Id
                        });

                        var supplyProduct = await _db.SupplyProducts
                            .AddAsync(new SupplyProduct()
                            {
                                ProductId = createProduct.Entity.Id,
                                SupplierId = supplierId,
                                RealizationAmount = (SupplyType) realization == SupplyType.ForRealization
                                    ? product.Amount
                                    : 0,
                                TotalAmount = product.Amount,
                                AdditionalCost = additionalCost,
                                ProcurementCost = product.Price,
                                FinalCost = product.Price + (additionalCost / product.Amount),
                                StockAmount = product.Amount,
                                SupplyHistoryId = supplyHistory.Entity.Id
                            });

                        if ((SupplyType) realization == SupplyType.ForRealization)
                            await _db.DeferredSupplyProducts.AddAsync(new DeferredSupplyProduct()
                            {
                                Date = date,
                                SupplyProductId = supplyProduct.Entity.Id
                            });
                    }
                    else
                    {
                        var supplyProduct = await _db.SupplyProducts
                            .AddAsync(new SupplyProduct()
                            {
                                ProductId = existingProduct.Id,
                                RealizationAmount = (SupplyType) realization == SupplyType.ForRealization
                                    ? product.Amount
                                    : 0,
                                TotalAmount = product.Amount,
                                AdditionalCost = additionalCost / product.Amount,
                                ProcurementCost = product.Price,
                                FinalCost = product.Price + (additionalCost / product.Amount),
                                StockAmount = product.Amount,
                                SupplyHistoryId = supplyHistory.Entity.Id
                            });

                        await _db.InfoProducts.AddAsync(new InfoProduct()
                        {
                            Amount = product.Amount,
                            Date = DateTime.Now.AddHours(3),
                            Product = existingProduct,
                            Type = InfoProductType.Supply,
                            ShopId = shopId,
                            SupplyHistoryId = supplyHistory.Entity.Id
                        });

                        if ((SupplyType) realization == SupplyType.DeferredPayment)
                            await _db.DeferredSupplyProducts.AddAsync(new DeferredSupplyProduct()
                            {
                                Date = date,
                                SupplyProductId = supplyProduct.Entity.Id
                            });
                    }
                }

                await _db.SaveChangesAsync();

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}