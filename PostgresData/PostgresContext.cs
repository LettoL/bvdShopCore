using System.Security.Cryptography.X509Certificates;
using Domain.Entities;
using Domain.Entities.Olds;
using Domain.Entities.Products;
using Domain.Entities.Sales;
using Domain.Entities.Supplies;
using Microsoft.EntityFrameworkCore;

namespace PostgresData
{
    public class PostgresContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<RepaidDebtOld> RepaidDebtsOld { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<DeletedManager> DeletedManagers { get; set; }
        
        public DbSet<SaleManagerOld> SaleManagersOld { get; set; }
        public DbSet<BookingManagerOld> BookingManagersOld { get; set; }
        public DbSet<SaleFromStockOld> SalesFromStockOld { get; set; }
        public DbSet<SaleOld> SalesOld { get; set; }
        public DbSet<DeletedSaleInfoOld> DeletedSalesInfoOld { get; set; }
        public DbSet<SupplyInfo> SuppliesInfo { get; set; }
        public DbSet<IncompleteProduct> IncompleteProducts { get; set; }

        public DbSet<SoldFromSupply> SoldFromSupplies { get; set; }
        public DbSet<ExpenseOld> ExpensesOld { get; set; }
        public DbSet<ProductOperation> ProductOperations { get; set; }
        public DbSet<SupplierPayment> SupplierPayments { get; set; }
        public DbSet<SupplierInfoInit> SupplierInfoInits { get; set; }
        public DbSet<ProductAmountInit> ProductAmountInits { get; set; }
        public DbSet<ArchiveCardKeeper> ArchiveCardKeepers { get; set; }
        public DbSet<ArchiveCalculatedScore> ArchiveCalculatedScores { get; set; }
        public DbSet<ManagerPayment> ManagerPayments { get; set; }
        public DbSet<ScheduledProductDelivery> ScheduledProductDeliveries { get; set; }
        public DbSet<ScheduledDelivery> ScheduledDeliveries { get; set; }
        public DbSet<ScheduledDeliveryPayment> ScheduledDeliveryPayments { get; set; }
        public DbSet<SupplierInfo> SuppliersInfos { get; set; }
        
        //public PostgresContext(DbContextOptions<PostgresContext> options) : base(options) {}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("");
        }
        
        private void ProvideClientCertificates(X509CertificateCollection clientCerts)
        {
            var clientCertPath = @"C:/ca-certificate.crt";
            var cert = new X509Certificate2(clientCertPath);
            clientCerts.Add(cert);
        }
    }
}