using System.Linq;
using Data;
using Domain.Entities;
using Handlers.Commands;
using PostgresData;

namespace Handlers.CommandHandlers
{
    public static class ScheduledDeliveryEditHandler
    {
        public static void Execute(EditScheduledDelivery command, PostgresContext postgresContext)
        {
            foreach (var scheduledDeliveryProduct in command.Products)
            {
                EditProduct(scheduledDeliveryProduct, postgresContext);
            }

            postgresContext.SaveChanges();
        }

        private static void EditProduct(EditedScheduledDeliveryProduct product, PostgresContext postgresContext)
        {
            if (IsNewScheduledProductDelivery(product))
            {
                var existingScheduledDeliveryProduct = postgresContext.ScheduledProductDeliveries
                    .FirstOrDefault(x => x.Id == product.Prev);

                var createdScheduledProductDelivery = postgresContext.ScheduledProductDeliveries.Add(
                    new ScheduledProductDelivery(
                        existingScheduledDeliveryProduct.ProductId,
                        product.Amount,
                        existingScheduledDeliveryProduct.SupplierId,
                        existingScheduledDeliveryProduct.ProcurementCost,
                        existingScheduledDeliveryProduct.ScheduledDeliveryId)).Entity;

                createdScheduledProductDelivery.ShopId = product.ShopId;

                if (product.Confirmed)
                    createdScheduledProductDelivery.DeliveryType = ScheduledProductDeliveryType.Delivered;
            }
            else
            {
                var existingScheduledProductDelivery = postgresContext.ScheduledProductDeliveries
                    .FirstOrDefault(x => x.Id == product.ProductDeliveryId);

                existingScheduledProductDelivery.ShopId = product.ShopId;

                if (existingScheduledProductDelivery.Amount != product.Amount)
                    existingScheduledProductDelivery.Amount = product.Amount;

                if (product.Confirmed)
                    existingScheduledProductDelivery.DeliveryType = ScheduledProductDeliveryType.Delivered;
            }
        }

        private static bool IsNewScheduledProductDelivery(EditedScheduledDeliveryProduct product) => product.Prev > 0;
    }
}