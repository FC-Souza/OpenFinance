using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using WebAPI.OpenFinance.Models;

namespace WebAPI.OpenFinance.Data
{
    public class OpenFinanceContext : DbContext
    {
        public OpenFinanceContext(DbContextOptions<OpenFinanceContext> options) : base(options) 
        { 
        }

        public DbSet<BanksModel> Banks { get; set; }
        public DbSet<ClientsModel> Clients { get; set; }
        public DbSet<ConnectionsModel> Connections { get; set; }
        public DbSet<ClientCredentialModel> ClientCredentials { get; set; }

        public DbSet<ProductTypesModel> ProductsTypes { get; set; }

        //Cash tables
        public DbSet<CashModel> Cash { get; set; }
        public DbSet<CashInfoModel> CashInfo { get; set; }


        //Stock tables
        public DbSet<StockModel> Stock { get; set; }
        public DbSet<StockInfoModel> StockInfo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Default value set to 3 for the remainingLoginAttempts
            modelBuilder.Entity<ClientCredentialModel>(entity =>
            {
                entity.Property(e => e.remainingLoginAttempts)
                      .HasDefaultValue(3);
            });
        }
    }
}
