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
        
        public DbSet<SaleManagerOld> SaleManagersOld { get; set; }
        public DbSet<BookingManagerOld> BookingManagersOld { get; set; }
        public DbSet<SaleFromStockOld> SalesFromStockOld { get; set; }
        public DbSet<SaleOld> SalesOld { get; set; }
        public DbSet<DeletedSaleInfoOld> DeletedSalesInfoOld { get; set; }
        public DbSet<SupplyInfo> SuppliesInfo { get; set; }
        public DbSet<IncompleteProduct> IncompleteProducts { get; set; }

        public DbSet<SoldFromSupply> SoldFromSupplies { get; set; }
        public DbSet<ExpenseOld> ExpensesOld { get; set; }
        
        //public PostgresContext(DbContextOptions<PostgresContext> options) : base(options) {}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
                "Host=localhost;Port=5432;Database=bvdSystem;Username=postgres;Password=565453749"
                //"Host=db-postgresql-fra1-35076-do-user-6959872-0.a.db.ondigitalocean.com;Port=25060;Database=defaultdb;Username=doadmin;Password=zu0qkbedxe6ma0ot;Ssl Mode=Require;Trust Server Certificate=true"
                //b => b.ProvideClientCertificatesCallback(ProvideClientCertificates)
            );
        }
        
        private void ProvideClientCertificates(X509CertificateCollection clientCerts)
        {
            var clientCertPath = @"C:/ca-certificate.crt";
            var cert = new X509Certificate2(clientCertPath);
            clientCerts.Add(cert);
        }
    }
}