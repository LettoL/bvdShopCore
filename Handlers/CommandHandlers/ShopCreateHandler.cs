using System.Threading.Tasks;
using Domain.Entities;
using Handlers.Commands;
using PostgresData;

namespace Handlers.CommandHandlers
{
    public static class ShopCreateHandler
    {
        public static async Task<int> Handler(this ShopCreate command, PostgresContext db)
        {
            var createTask = await db.Shops.AddAsync(new Shop(command.Title));

            await db.SaveChangesAsync();

            return createTask.Entity.Id;
        }
    }
}