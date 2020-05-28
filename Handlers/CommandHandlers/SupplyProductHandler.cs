using System;
using System.Threading.Tasks;
using Handlers.Commands;
using PostgresData;

namespace Handlers.CommandHandlers
{
    public static class SupplyProductHandler
    {
        public static async Task<int> Handle(this SupplyProduct command, PostgresContext db)
        {
            var currentDate = DateTime.UtcNow;
            
            var suppliedProduct = command.CreateSuppliedProduct(currentDate);

            var suppliedProductSave = await db.SuppliedProducts.AddAsync(suppliedProduct);

            await db.SaveChangesAsync();
            
            return suppliedProductSave.Entity.Id;
        }
    }
}