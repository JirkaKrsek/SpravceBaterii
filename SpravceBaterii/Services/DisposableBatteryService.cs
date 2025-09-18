using Microsoft.EntityFrameworkCore;
using SpravceBaterii.Data;
using SpravceBaterii.Data.Models;

namespace SpravceBaterii.Services
{
    public class DisposableBatteryService
    {
        private readonly ApplicationDbContext ApplicationDbContext;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="applicationDbContext">ApplicationDbContext</param>
        public DisposableBatteryService(ApplicationDbContext applicationDbContext)
        {
            ApplicationDbContext = applicationDbContext;
        }

        /// <summary>
        /// Získání jednorázové baterie podle ID baterie
        /// </summary>
        /// <param name="batteryId">ID baterie</param>
        /// <returns>Jednorázová baterie / null</returns>
        public async Task<DisposableBattery?> GetDisposableBatteryById(int batteryId)
        {
            return await ApplicationDbContext.DisposableBatteries.AsNoTracking().FirstOrDefaultAsync(b => b.BatteryId == batteryId);
        }

        /// <summary>
        /// Přidání jednorázové baterie do databáze
        /// </summary>
        /// <param name="disposableBattery">Nová jednorázová baterie</param>
        /// <returns>Asynchronní operace</returns>
        public async Task Add(DisposableBattery disposableBattery)
        {
            ApplicationDbContext.DisposableBatteries.Add(disposableBattery);
            //Uložení
            await ApplicationDbContext.SaveChangesAsync();

            //Odpojení od slednování EF Core
            ApplicationDbContext.Entry(disposableBattery).State = EntityState.Detached;
        }

        /// <summary>
        /// Aktualizace jednorázové baterie
        /// </summary>
        /// <param name="disposableBattery">Existující jednorázová baterie</param>
        /// <returns>Asynchronní operace</returns>
        public async Task Update(DisposableBattery disposableBattery)
        {
            ApplicationDbContext.DisposableBatteries.Update(disposableBattery);
            //Uložení
            await ApplicationDbContext.SaveChangesAsync();

            //Odpojení od slednování EF Core
            ApplicationDbContext.Entry(disposableBattery).State = EntityState.Detached;
        }

        /// <summary>
        /// Odstranění jednorázové baterie z databáze
        /// </summary>
        /// <param name="battery">Požadovaná jednorázová baterie</param>
        /// <returns>Asynchronní operace</returns>
        public async Task Delete(DisposableBattery battery)
        {
            ApplicationDbContext.DisposableBatteries.Remove(battery);
            await ApplicationDbContext.SaveChangesAsync();
        }
    }
}
