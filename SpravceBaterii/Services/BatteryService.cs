using Microsoft.EntityFrameworkCore;
using SpravceBaterii.Data;
using SpravceBaterii.Data.Models;

namespace SpravceBaterii.Services
{
    public class BatteryService
    {
        private readonly ApplicationDbContext ApplicationDbContext;
        private readonly ApplicationUserService UserService;
        private readonly DisposableBatteryService DisposableBatteryService;
        private readonly RechargeableBatteryService RechargeableBatteryService;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="applicationDbContext">ApplicationDbContext</param>
        /// <param name="userService">ApplicationUserService</param>
        /// <param name="disposableBatteryService">DisposableBatteryService</param>
        /// <param name="rechargeableBatteryService">RechargeableBatteryService</param>
        public BatteryService(ApplicationDbContext applicationDbContext, ApplicationUserService userService, DisposableBatteryService disposableBatteryService, RechargeableBatteryService rechargeableBatteryService)
        {
            ApplicationDbContext = applicationDbContext;
            UserService = userService;
            DisposableBatteryService = disposableBatteryService;
            RechargeableBatteryService = rechargeableBatteryService;
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
        /// Získání baterie podle ID
        /// </summary>
        /// <param name="batteryId">ID hledané baterie</param>
        /// <returns>Nalezená baterie</returns>
        /// <exception cref="InvalidOperationException">Baterie nenalezena</exception>
        public async Task<Battery> GetUserBatteryById(int batteryId)
        {
            string userId = await UserService.GetUserIdAsync();

            return await ApplicationDbContext.Batteries
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.UserId == userId && b.Id == batteryId)
                ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Aktualizace baterie v databázi
        /// </summary>
        /// <param name="battery">Upravená baterie</param>
        /// <returns>Asynchronní operace</returns>
        /// <exception cref="InvalidOperationException">Neplatná data</exception>
        public async Task UpdateBattery(Battery battery)
        {
            string userId = await UserService.GetUserIdAsync();

            if (battery.UserId == userId)
            {
                DisposableBattery? disposableBattery = battery.DisposableBattery;
                RechargeableBattery? rechargeableBattery = battery.RechargeableBattery;

                battery.DisposableBattery = null;
                battery.RechargeableBattery = null;

                ApplicationDbContext.Update(battery);
                //Uložení
                await ApplicationDbContext.SaveChangesAsync();


                RechargeableBattery? existingRechargeableBattery = await RechargeableBatteryService.GetRechargeableBatteryById(battery.Id);
                DisposableBattery? existingDisposableBattery = await DisposableBatteryService.GetDisposableBatteryById(battery.Id);
                
                if (battery.IsRechargeable && rechargeableBattery is not null)
                {
                    rechargeableBattery.BatteryId = battery.Id;

                    if (existingRechargeableBattery is not null)
                    {
                        rechargeableBattery.Id = existingRechargeableBattery.Id;
                        await RechargeableBatteryService.Update(rechargeableBattery);
                    }
                    else
                    {
                        await RechargeableBatteryService.Add(rechargeableBattery);
                        if (existingDisposableBattery is not null)
                        {
                            await DisposableBatteryService.Delete(existingDisposableBattery);
                        }
                    }
                    //Uložení
                    await ApplicationDbContext.SaveChangesAsync();

                    //Odpojení od slednování EF Core
                    ApplicationDbContext.Entry(rechargeableBattery).State = EntityState.Detached;
                }
                else if (!battery.IsRechargeable && disposableBattery is not null)
                {
                    disposableBattery.BatteryId = battery.Id;

                    if (existingDisposableBattery is not null)
                    {
                        disposableBattery.Id = existingDisposableBattery.Id;
                        await DisposableBatteryService.Update(disposableBattery);
                    }
                    else
                    {
                        await DisposableBatteryService.Add(disposableBattery);
                        if (existingRechargeableBattery is not null)
                        {
                            await RechargeableBatteryService.Delete(existingRechargeableBattery);
                        }
                    }
                    //Uložení
                    await ApplicationDbContext.SaveChangesAsync();

                    //Odpojení od slednování EF Core
                    ApplicationDbContext.Entry(disposableBattery).State = EntityState.Detached;
                }
                else
                {
                    //Odpojení od slednování EF Core
                    ApplicationDbContext.Entry(battery).State = EntityState.Detached;

                    throw new InvalidOperationException();
                }

                //Odpojení od slednování EF Core
                ApplicationDbContext.Entry(battery).State = EntityState.Detached;
            }
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
