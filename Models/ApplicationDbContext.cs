using Microsoft.EntityFrameworkCore;

namespace GasStationApp
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<StorageInfo> Storages { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<FuelPrice> FuelPrices {get; set;}
        public DbSet<SaleRecord> SaleRecords {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FuelPrice>().HasData(
                new FuelPrice { Id = 1, GasType = 1, PricePerLiter = 20.5 },
                new FuelPrice { Id = 2, GasType = 2, PricePerLiter = 18.3 },
                new FuelPrice { Id = 3, GasType = 3, PricePerLiter = 22.1 },
                new FuelPrice { Id = 4, GasType = 4, PricePerLiter = 21.0 },
                new FuelPrice { Id = 5, GasType = 5, PricePerLiter = 19.5 }
            );

            modelBuilder.Entity<StorageInfo>().HasData(
                new StorageInfo {Id=1, GasType=1, Occupancy=0},
                new StorageInfo {Id=2, GasType=2, Occupancy=0},
                new StorageInfo {Id=3, GasType=3, Occupancy=0},
                new StorageInfo {Id=4, GasType=4, Occupancy=0},
                new StorageInfo {Id=5, GasType=5, Occupancy=0}
            );
        }
        
    }
}
