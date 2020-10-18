using System.Threading.Tasks;
using Domain.Commands.Supplies;
using Domain.Entities.Supplies;
using PostgresData;

namespace Handlers.CommandHandlers
{
    public static class SupplyProductHandler
    {
       /* public static async Task<SuppliedProduct> Execute(this SupplyProduct command, PostgresContext db)
        {
            var create = await db.SuppliedProducts
                .AddAsync(new SuppliedProduct(command));

            await db.SaveChangesAsync();

            return create.Entity;
        }*/
    }
}