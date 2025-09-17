using Microsoft.EntityFrameworkCore;
using SpravceBaterii.Data;
using SpravceBaterii.Data.Models;

namespace SpravceBaterii.Services
{
    public class BatteryService
    {
        private ApplicationDbContext ApplicationDbContext { get; set; }
        private readonly ApplicationUserService UserService;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="applicationDbContext">ApplicationDbContext</param>
        /// <param name="userService">ApplicationUserService</param>
        public BatteryService(ApplicationDbContext applicationDbContext, ApplicationUserService userService)
        {
            ApplicationDbContext = applicationDbContext;
            UserService = userService;
        }

        /// <summary>
        /// Načtení baterií pro daného uživatele podle jeho ID
        /// </summary>
        /// <returns>List baterií</returns>
        public async Task<List<Battery>> GetUserBatteries()
        {
            string userId = await UserService.GetUserIdAsync();

            return await ApplicationDbContext.Batteries
                .Where(b => b.UserId == userId)
                .Include(b => b.Device)
                .Include(b => b.BatteryType)
                .Include(b => b.DisposableBattery)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Přidání nové baterie do databáze
        /// </summary>
        /// <param name="battery">Zadaná baterie</param>
        /// <returns>Asynchronní operace</returns>
        /// <exception cref="InvalidOperationException">Neplatný typ baterie</exception>
        public async Task AddBattery(Battery battery)
        {
            string userId = await UserService.GetUserIdAsync();
            battery.UserId = userId;

            if (battery.IsRechargeable && battery.RechargeableBattery is not null)
            {
                battery.DisposableBattery = null;
            }
            else if (!battery.IsRechargeable && battery.DisposableBattery is not null)
            {
                battery.RechargeableBattery = null;
            }
            else
            {
                throw new InvalidOperationException();
            }

            ApplicationDbContext.Batteries.Add(battery);
            //Uložení
            await ApplicationDbContext.SaveChangesAsync();

            //Odpojení od slednování EF Core
            ApplicationDbContext.Entry(battery).State = EntityState.Detached;
        }
    }
}
