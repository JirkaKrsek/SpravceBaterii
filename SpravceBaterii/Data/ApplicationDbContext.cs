using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SpravceBaterii.Data.Models;

namespace SpravceBaterii.Data
{
    /// <summary>
    /// Třída pro správu databáze a identity uživatelů pomocí závislostí Microsoft.EntityFrameworkCore a Microsoft.AspNetCore.Identity
    /// </summary>
    public class ApplicationDbContext : IdentityUserContext<ApplicationUser>
    {
        /// <summary>
        /// Konstruktor s konfigurací připojení
        /// </summary>
        /// <param name="options"></param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        
        /// <summary>
        /// Připojení modelových tříd
        /// </summary>
        public DbSet<Battery> Batteries { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<BatteryType> BatteryTypes { get; set; }
        public DbSet<ChemicalComposition> ChemicalCompositions { get; set; }
        public DbSet<DisposableBattery> DisposableBatteries { get; set; }
        public DbSet<RechargeableBattery> RechargeableBatteries { get; set; }
        public DbSet<ChargingHistory> ChargingHistories { get; set; }
    }
}
