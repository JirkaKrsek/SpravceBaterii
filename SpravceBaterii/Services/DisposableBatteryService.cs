using Microsoft.EntityFrameworkCore;
using SpravceBaterii.Data;
using SpravceBaterii.Data.Models;

namespace SpravceBaterii.Services
{
    public class DisposableBatteryService
    {
        private readonly ApplicationDbContext applicationDbContext;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="applicationDbContext">ApplicationDbContext</param>
        public DisposableBatteryService(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        /// <summary>
        /// Získání jednorázové baterie podle ID baterie
        /// </summary>
        /// <param name="batteryId">ID baterie</param>
        /// <returns>Jednorázová baterie / null</returns>
        public async Task<DisposableBattery?> GetDisposableBatteryById(int batteryId)
        {
            return await applicationDbContext.DisposableBatteries.AsNoTracking().FirstOrDefaultAsync(b => b.BatteryId == batteryId);
        }

        /// <summary>
        /// Přidání jednorázové baterie do databáze
        /// </summary>
        /// <param name="disposableBattery">Nová jednorázová baterie</param>
        /// <returns>Asynchronní operace</returns>
        public async Task Add(DisposableBattery disposableBattery)
        {
            applicationDbContext.DisposableBatteries.Add(disposableBattery);
            //Uložení
            await applicationDbContext.SaveChangesAsync();

            //Odpojení všech entit od slednování v EF Core
            applicationDbContext.ChangeTracker.Clear();
        }

        /// <summary>
        /// Aktualizace jednorázové baterie
        /// </summary>
        /// <param name="disposableBattery">Existující jednorázová baterie</param>
        /// <returns>Asynchronní operace</returns>
        public async Task Update(DisposableBattery disposableBattery)
        {
            applicationDbContext.DisposableBatteries.Update(disposableBattery);
            //Uložení
            await applicationDbContext.SaveChangesAsync();

            //Odpojení všech entit od slednování v EF Core
            applicationDbContext.ChangeTracker.Clear();
        }

        /// <summary>
        /// Odstranění jednorázové baterie z databáze
        /// </summary>
        /// <param name="battery">Požadovaná jednorázová baterie</param>
        /// <returns>Asynchronní operace</returns>
        public async Task Delete(DisposableBattery battery)
        {
            applicationDbContext.DisposableBatteries.Remove(battery);
            //Uložení
            await applicationDbContext.SaveChangesAsync();

            //Odpojení všech entit od slednování v EF Core
            applicationDbContext.ChangeTracker.Clear();
        }
    }
}
