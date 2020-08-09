using System;
using System.Threading.Tasks;
using Domain.Commands;
using Domain.Entities.Products;
using PostgresData;

namespace Handlers.CommandHandlers
{
    public static class SupplyProductHandler
    {
        public static async Task<SuppliedProduct> Handle(this SupplyProduct command, PostgresContext db)
        {
            var currentDate = DateTime.UtcNow;

            var createTask = await db.SuppliedProducts
                .AddAsync(SuppliedProduct.Create(command, currentDate));

            await db.SaveChangesAsync();
            
            return createTask.Entity;
        }
    }
}