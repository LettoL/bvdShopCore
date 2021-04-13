using System;
using System.Linq;
using Data;
using Data.Entities;
using Data.Enums;
using Domain.Entities;
using Domain.Entities.Olds;
using PostgresData;

namespace Handlers.CommandHandlers
{
    public static class ScheduledDeliveryConfirmHandler
    {
        public static void Execute(int deliveryId, PostgresContext postgresContext, ShopContext shopContext)
        {
            var deliveredProducts = postgresContext.ScheduledProductDeliveries
                .Where(x => x.ScheduledDeliveryId == deliveryId
                            && x.SupplyProductId == null
                            && x.DeliveryType == ScheduledProductDeliveryType.Delivered)
                .ToList();

            var supplyHistory = shopContext.SupplyHistories.Add(new SupplyHistory()).Entity;
            
            foreach (var deliveredProduct in deliveredProducts)
            {
                DeliverProduct(deliveredProduct, supplyHistory, postgresContext, shopContext);
            }

            postgresContext.SaveChanges();
            shopContext.SaveChanges();
        }

        public static void DeliverProduct(ScheduledProductDelivery deliveredProduct, SupplyHistory supplyHistory,
            PostgresContext postgresContext, ShopContext shopContext)
        {
            var product = GetProduct(deliveredProduct.ProductId, deliveredProduct.ShopId, shopContext);

            shopContext.InfoProducts.Add(new InfoProduct()
            {
                Amount = deliveredProduct.Amount,
                Date = DateTime.Now.AddHours(3),
                Product = product,
                SupplierId = deliveredProduct.SupplierId,
                Type = InfoProductType.Supply,
                ShopId = deliveredProduct.ShopId,
                SupplyHistory = supplyHistory
            });

            var supplyProduct = shopContext.SupplyProducts.Add(new SupplyProduct()
            {
                Product = product,
                SupplierId = deliveredProduct.SupplierId,
                RealizationAmount = 0,
                TotalAmount = deliveredProduct.Amount,
                AdditionalCost = 0,
                ProcurementCost = deliveredProduct.ProcurementCost,
                FinalCost = deliveredProduct.ProcurementCost,
                StockAmount = deliveredProduct.Amount,
                SupplyHistory = supplyHistory
            });

            shopContext.SaveChanges();

            deliveredProduct.SupplyProductId = supplyProduct.Entity.Id;
            
            postgresContext.ProductOperations.Add(new ProductOperation(
                supplyProduct.Entity.Id,
                deliveredProduct.Amount,
                DateTime.Now.AddHours(3),
                deliveredProduct.ProcurementCost,
                false,
                deliveredProduct.SupplierId,
                StorageType.Shop));
        }

        private static Product GetProduct(int productId, int shopId, ShopContext shopContext)
        {
            var product = shopContext.Products
                .FirstOrDefault(x => x.Id == productId);

            if (product.ShopId == shopId)
                return product;

            var existingProduct = shopContext.Products
                .FirstOrDefault(x => x.Title == product.Title
                                     && x.ShopId == shopId);

            if (existingProduct != null)
                return existingProduct;
            
            var createdProduct = shopContext.Products.Add(new Product()
            {
                Code = product.Code,
                Title = product.Title,
                ShopId = shopId,
                CategoryId = product.CategoryId,
                Cost = product.Cost
            });

            return createdProduct.Entity;
        }
    }
}