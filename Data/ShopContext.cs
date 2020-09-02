using System;
using Base;
using Base.Services.Abstract;
using Data.Entities;
using Data.Enums;
using Data.ViewModels.Olds;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class ShopContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleProduct> SalesProducts { get; set; }
        public DbSet<InfoMoney> InfoMonies { get; set; }
        public DbSet<InfoProduct> InfoProducts { get; set; }
        public DbSet<SupplyProduct> SupplyProducts { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingProduct> BookingProducts { get; set; }
        public DbSet<SupplyHistory> SupplyHistories { get; set; }
        public DbSet<CalculatedScore> CalculatedScores { get; set; }
        public DbSet<CardKeeper> CardKeepers { get; set; }
        public DbSet<DeferredSupplyProduct> DeferredSupplyProducts { get; set; }
        public DbSet<MoneyWorker> MoneyWorkers { get; set; }
        public DbSet<SaleInformation> SaleInformations { get; set; }
        public DbSet<ProductInformation> ProductInformations { get; set; }
        public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<MoneyTransfer> MoneyTransfers { get; set; }
        
        
        public DbSet<AvailableProduct> AvailableProducts { get; set; }

        public ShopContext(DbContextOptions<ShopContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AvailableProduct>().HasNoKey();
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
