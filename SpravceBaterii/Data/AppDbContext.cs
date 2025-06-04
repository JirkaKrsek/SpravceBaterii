using Microsoft.EntityFrameworkCore;

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
    }
}
