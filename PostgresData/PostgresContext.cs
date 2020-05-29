using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace PostgresData
{
    public class PostgresContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SuppliedProduct> SuppliedProducts { get; set; }

        //public PostgresContext(DbContextOptions<PostgresContext> options) : base(options) {}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
                "Host=localhost;Port=5432;Database=bvdSystem;Username=postgres;Password=565453749");
        }
    }
}