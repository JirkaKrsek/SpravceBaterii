using Microsoft.EntityFrameworkCore;
using SpravceBaterii.Data;
using SpravceBaterii.Data.Models;

namespace SpravceBaterii.Services
{
    public class RechargeableBatteryService
    {
        private readonly ApplicationDbContext applicationDbContext;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="applicationDbContext">ApplicationDbContext</param>
        public RechargeableBatteryService(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        /// <summary>
        /// Získání nabíjecí baterie podle ID baterie
        /// </summary>
        /// <param name="batteryId">ID baterie</param>
        /// <returns>Nabíjecí baterie / null</returns>
        public async Task<RechargeableBattery?> GetRechargeableBatteryById(int batteryId)
        {
            return await applicationDbContext.RechargeableBatteries.AsNoTracking().FirstOrDefaultAsync(b => b.BatteryId == batteryId);
        }

        /// <summary>
        /// Přidání nabíjecí baterie do databáze
        /// </summary>
        /// <param name="rechargeableBattery">Nová nabíjecí baterie</param>
        /// <returns>Asynchronní operace</returns>
        public async Task Add(RechargeableBattery rechargeableBattery)
        {
            applicationDbContext.RechargeableBatteries.Add(rechargeableBattery);
            //Uložení
            await applicationDbContext.SaveChangesAsync();

            //Odpojení od slednování EF Core
            applicationDbContext.Entry(rechargeableBattery).State = EntityState.Detached;
        }

        /// <summary>
        /// Aktualizace nabíjecí baterie
        /// </summary>
        /// <param name="rechargeableBattery">Existující nabíjecí baterie</param>
        /// <returns>Asynchronní operace</returns>
        public async Task Update(RechargeableBattery rechargeableBattery)
        {
            applicationDbContext.RechargeableBatteries.Update(rechargeableBattery);
            //Uložení
            await applicationDbContext.SaveChangesAsync();

            //Odpojení od slednování EF Core
            applicationDbContext.Entry(rechargeableBattery).State = EntityState.Detached;
        }

        /// <summary>
        /// Odstranění nabíjecí baterie z databáze
        /// </summary>
        /// <param name="battery">Požadovaná nabíjecí baterie</param>
        /// <returns>Asynchronní operace</returns>
        public async Task Delete(RechargeableBattery battery)
        {
            applicationDbContext.RechargeableBatteries.Remove(battery);
            await applicationDbContext.SaveChangesAsync();
        }
    }
}
