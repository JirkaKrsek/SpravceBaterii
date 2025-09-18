using Microsoft.EntityFrameworkCore;
using SpravceBaterii.Data;
using SpravceBaterii.Data.Models;

namespace SpravceBaterii.Services
{
    public class BatteryTypeService
    {
        private readonly ApplicationDbContext ApplicationDbContext;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="applicationDbContext">ApplicationDbContext</param>
        public BatteryTypeService(ApplicationDbContext applicationDbContext)
        {
            ApplicationDbContext = applicationDbContext;
        }

        /// <summary>
        /// Načtení všech typů baterií
        /// </summary>
        /// <returns>List typů baterií</returns>
        public async Task<List<BatteryType>> GetBatteryTypes()
        {
            return await ApplicationDbContext.BatteryTypes.ToListAsync();
        }
    }
}
