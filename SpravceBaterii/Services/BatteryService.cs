using Microsoft.EntityFrameworkCore;
using SpravceBaterii.Data;
using SpravceBaterii.Data.Models;

namespace SpravceBaterii.Services
{
    public class BatteryService
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly ApplicationUserService userService;
        private readonly DisposableBatteryService disposableBatteryService;
        private readonly RechargeableBatteryService rechargeableBatteryService;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="applicationDbContext">ApplicationDbContext</param>
        /// <param name="userService">ApplicationUserService</param>
        /// <param name="disposableBatteryService">DisposableBatteryService</param>
        /// <param name="rechargeableBatteryService">RechargeableBatteryService</param>
        public BatteryService(ApplicationDbContext applicationDbContext, ApplicationUserService userService, DisposableBatteryService disposableBatteryService, RechargeableBatteryService rechargeableBatteryService)
        {
            this.applicationDbContext = applicationDbContext;
            this.userService = userService;
            this.disposableBatteryService = disposableBatteryService;
            this.rechargeableBatteryService = rechargeableBatteryService;
        }

        /// <summary>
        /// Načtení baterií pro daného uživatele podle jeho ID
        /// </summary>
        /// <returns>List baterií</returns>
        public async Task<List<Battery>> GetUserBatteries()
        {
            string userId = await userService.GetUserIdAsync();

            return await applicationDbContext.Batteries
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
        /// <exception cref="KeyNotFoundException">Baterie nenalezena</exception>
        public async Task<Battery> GetUserBatteryById(int batteryId)
        {
            string userId = await userService.GetUserIdAsync();

            return await applicationDbContext.Batteries
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.UserId == userId && b.Id == batteryId)
                ?? throw new KeyNotFoundException();
        }

        /// <summary>
        /// Načtení baterií podle ID zařízení, ve kterém jsou vloženy
        /// </summary>
        /// <param name="deviceId">ID zařízení</param>
        /// <returns>List baterií</returns>
        public async Task<List<Battery>> GetUserBatteriesByDeviceId(int deviceId)
        {
            string userId = await userService.GetUserIdAsync();

            return await applicationDbContext.Batteries
                .Where(b => b.UserId == userId && b.DeviceId == deviceId)
                .Include(b => b.BatteryType)
                .Include(b => b.DisposableBattery)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Načtení baterií, které nejsou vloženy v žádném zařízení, podle ID jejich typu
        /// </summary>
        /// <param name="typeId">ID typu baterie</param>
        /// <returns>List baterií</returns>
        public async Task<List<Battery>> GetUnUsedBatteriesByTypeId(int typeId)
        {
            string userId = await userService.GetUserIdAsync();

            return await applicationDbContext.Batteries
                .Where(b => b.UserId == userId && b.DeviceId == null && b.BatteryTypeId == typeId)
                .Include(b => b.BatteryType)
                .Include(b => b.DisposableBattery)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Aktualizace samotné baterie v databázi
        /// </summary>
        /// <param name="battery">Upravená baterie</param>
        /// <returns>Asynchronní operace</returns>
        /// <exception cref="UnauthorizedAccessException">Uživatel nemá oprávnění</exception>
        public async Task UpdateOnlyBattery(Battery battery)
        {
            string userId = await userService.GetUserIdAsync();

            if (battery.UserId == userId)
            {
                applicationDbContext.Update(battery);
                //Uložení
                await applicationDbContext.SaveChangesAsync();

                //Odpojení od slednování EF Core
                applicationDbContext.Entry(battery).State = EntityState.Detached;
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        /// <summary>
        /// Aktualizace baterie v databázi
        /// </summary>
        /// <param name="battery">Upravená baterie</param>
        /// <returns>Asynchronní operace</returns>
        /// <exception cref="InvalidOperationException">Neplatná data</exception>
        /// <exception cref="UnauthorizedAccessException">Uživatel nemá oprávnění</exception>
        public async Task UpdateBattery(Battery battery)
        {
            string userId = await userService.GetUserIdAsync();

            if (battery.UserId == userId)
            {
                DisposableBattery? disposableBattery = battery.DisposableBattery;
                RechargeableBattery? rechargeableBattery = battery.RechargeableBattery;

                battery.DisposableBattery = null;
                battery.RechargeableBattery = null;

                applicationDbContext.Update(battery);
                //Uložení
                await applicationDbContext.SaveChangesAsync();


                RechargeableBattery? existingRechargeableBattery = await rechargeableBatteryService.GetRechargeableBatteryById(battery.Id);
                DisposableBattery? existingDisposableBattery = await disposableBatteryService.GetDisposableBatteryById(battery.Id);

                if (battery.IsRechargeable && rechargeableBattery is not null)
                {
                    rechargeableBattery.BatteryId = battery.Id;

                    if (existingRechargeableBattery is not null)
                    {
                        rechargeableBattery.Id = existingRechargeableBattery.Id;
                        await rechargeableBatteryService.Update(rechargeableBattery);
                    }
                    else
                    {
                        await rechargeableBatteryService.Add(rechargeableBattery);
                        if (existingDisposableBattery is not null)
                        {
                            await disposableBatteryService.Delete(existingDisposableBattery);
                        }
                    }
                    //Uložení
                    await applicationDbContext.SaveChangesAsync();

                    //Odpojení od slednování EF Core
                    applicationDbContext.Entry(rechargeableBattery).State = EntityState.Detached;
                }
                else if (!battery.IsRechargeable && disposableBattery is not null)
                {
                    disposableBattery.BatteryId = battery.Id;

                    if (existingDisposableBattery is not null)
                    {
                        disposableBattery.Id = existingDisposableBattery.Id;
                        await disposableBatteryService.Update(disposableBattery);
                    }
                    else
                    {
                        await disposableBatteryService.Add(disposableBattery);
                        if (existingRechargeableBattery is not null)
                        {
                            await rechargeableBatteryService.Delete(existingRechargeableBattery);
                        }
                    }
                    //Uložení
                    await applicationDbContext.SaveChangesAsync();

                    //Odpojení od slednování EF Core
                    applicationDbContext.Entry(disposableBattery).State = EntityState.Detached;
                }
                else
                {
                    //Odpojení od slednování EF Core
                    applicationDbContext.Entry(battery).State = EntityState.Detached;

                    throw new InvalidOperationException();
                }

                //Odpojení od slednování EF Core
                applicationDbContext.Entry(battery).State = EntityState.Detached;
            }
            else
            {
                throw new UnauthorizedAccessException();
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
            string userId = await userService.GetUserIdAsync();
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

            applicationDbContext.Batteries.Add(battery);
            //Uložení
            await applicationDbContext.SaveChangesAsync();

            //Odpojení od slednování EF Core
            applicationDbContext.Entry(battery).State = EntityState.Detached;
        }

        /// <summary>
        /// Odstranění baterie z databáze
        /// </summary>
        /// <param name="batteryId">ID baterie</param>
        /// <returns>Asynchronní operace</returns>
        /// <exception cref="UnauthorizedAccessException">Uživatel nemá oprávnění</exception>
        public async Task DeleteBatteryById(int batteryId)
        {
            string userId = await userService.GetUserIdAsync();
            Battery battery = await GetUserBatteryById(batteryId);

            if (battery.UserId == userId)
            {
                applicationDbContext.Remove(battery);
                //Uložení
                await applicationDbContext.SaveChangesAsync();
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        /// <summary>
        /// Vytvoření kopie objektu baterie
        /// </summary>
        /// <param name="sourceBattery">Zdrojová baterie</param>
        /// <returns>Kopie baterie</returns>
        public Battery CreateBatteryCopy(Battery sourceBattery)
        {
            RechargeableBattery rechargeableBattery = new();
            DisposableBattery disposableBattery = new();

            if (sourceBattery.IsRechargeable & sourceBattery.RechargeableBattery is not null)
            {
                rechargeableBattery = new()
                {
                    BatteryId = sourceBattery.RechargeableBattery!.BatteryId,
                    Capacity = sourceBattery.RechargeableBattery.Capacity,
                    CycleCount = sourceBattery.RechargeableBattery.CycleCount
                };
            }
            else if (!sourceBattery.IsRechargeable & sourceBattery.DisposableBattery is not null)
            {
                disposableBattery = new()
                {
                    BatteryId = sourceBattery.DisposableBattery!.BatteryId,
                    ExpirationDate = sourceBattery.DisposableBattery.ExpirationDate
                };
            }

            Battery batteryCopy = new()
            {
                BatteryTypeId = sourceBattery.BatteryTypeId,
                ChemicalCompositionId = sourceBattery.ChemicalCompositionId,
                IsRechargeable = sourceBattery.IsRechargeable,
                DisposableBattery = disposableBattery,
                RechargeableBattery = rechargeableBattery,
                Description = sourceBattery.Description,
                Manufacturer = sourceBattery.Manufacturer,
                ExpectedLifespan = sourceBattery.ExpectedLifespan,
                InsertionDate = sourceBattery.InsertionDate,
                UsageHistory = sourceBattery.UsageHistory,
                UserId = sourceBattery.UserId,
                DeviceId = sourceBattery.DeviceId,
                Count = sourceBattery.Count
            };

            return batteryCopy;
        }
    }
}
