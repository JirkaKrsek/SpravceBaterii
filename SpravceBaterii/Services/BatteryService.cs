using Microsoft.EntityFrameworkCore;
using SpravceBaterii.Data;
using SpravceBaterii.Data.Models;

namespace SpravceBaterii.Services
{
    public class BatteryService
    {
        private ApplicationDbContext ApplicationDbContext { get; set; }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="applicationDbContext">ApplicationDbContext</param>
        public BatteryService(ApplicationDbContext applicationDbContext)
        {
            ApplicationDbContext = applicationDbContext;
        }

        /// <summary>
        /// Načtení baterií pro daného uživatele podle jeho ID
        /// </summary>
        /// <param name="userId">ID uživatele</param>
        /// <returns>List baterií</returns>
        public async Task<List<Battery>> GetUserBatteries(string userId)
        {
            return await ApplicationDbContext.Batteries.Where(b => b.UserId == userId).ToListAsync();
        }
    }
}
