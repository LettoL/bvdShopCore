using System.Security.Cryptography.X509Certificates;
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
        public DbSet<RepaidDebtOld> RepaidDebtsOld { get; set; }
        public DbSet<Manager> Managers { get; set; }
        
        public DbSet<SaleManagerOld> SaleManagersOld { get; set; }
        public DbSet<SaleFromStockOld> SalesFromStockOld { get; set; }
        
        //public PostgresContext(DbContextOptions<PostgresContext> options) : base(options) {}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
                "Host=localhost;Port=5432;Database=bvdSystem;Username=postgres;Password=xxxxxxx",
                b => b.ProvideClientCertificatesCallback(ProvideClientCertificates)
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