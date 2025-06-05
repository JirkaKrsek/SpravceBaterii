using Microsoft.EntityFrameworkCore;
using SpravceBaterii.Data.Models;

namespace SpravceBaterii.Data
{
    /// <summary>
    /// Třída pro správu databáze pomocí závislosti Microsoft.EntityFrameworkCore
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Konstruktor s konfigurací připojení
        /// </summary>
        /// <param name="options"></param>
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Battery> Batteries { get; set; }
        public DbSet<BatteryType> BatteryTypes { get; set; }
        public DbSet<ChemicalComposition> ChemicalCompositions { get; set; }
        public DbSet<DisposableBattery> DisposableBatteries { get; set; }
        public DbSet<RechargeableBattery> RechargeableBatteries { get; set; }
        public DbSet<ChargingHistory> ChargingHistories { get; set; }
    }
}
