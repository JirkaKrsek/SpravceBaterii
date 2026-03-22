using Microsoft.EntityFrameworkCore;
using SpravceBaterii.Data;
using SpravceBaterii.Data.Models;

namespace SpravceBaterii.Services
{
    public class LocationService
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly ApplicationUserService userService;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="applicationDbContext">ApplicationDbContext</param>
        /// <param name="userService">ApplicationUserService</param>
        public LocationService(ApplicationDbContext applicationDbContext, ApplicationUserService userService)
        {
            this.applicationDbContext = applicationDbContext;
            this.userService = userService;
        }

        /// <summary>
        /// Načtení umístění přihlášeného uživatele
        /// </summary>
        /// <returns>List umístění</returns>
        public async Task<List<Location>> GetLocations()
        {
            string userId = await userService.GetUserIdAsync();

            return await applicationDbContext.Locations
                .Include(l => l.Devices)
                .AsNoTracking()
                .Where(l => l.UserId == userId).ToListAsync();
        }

        /// <summary>
        /// Získání umístění podle ID
        /// </summary>
        /// <param name="locationId">ID hledaného umístění</param>
        /// <returns>Nalezené umístění</returns>
        /// <exception cref="KeyNotFoundException">Umístění nenalezeno</exception>
        public async Task<Location> GetUserLocationById(int locationId)
        {
            string userId = await userService.GetUserIdAsync();

            return await applicationDbContext.Locations
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.UserId == userId && l.Id == locationId)
                ?? throw new KeyNotFoundException();
        }

        /// <summary>
        /// Uložené nového umístění do databáze
        /// </summary>
        /// <param name="location">Zadané umístění</param>
        /// <returns>Asynchronní operace</returns>
        public async Task AddLocation(Location location)
        {
            string userId = await userService.GetUserIdAsync();
            location.UserId = userId;

            applicationDbContext.Locations.Add(location);
            await applicationDbContext.SaveChangesAsync();

            //Odpojení všech entit od slednování v EF Core
            applicationDbContext.ChangeTracker.Clear();
        }

        /// <summary>
        /// Aktualizace umístění v databázi
        /// </summary>
        /// <param name="location">Upravené umístění</param>
        /// <returns>Asynchronní operace</returns>
        /// <exception cref="InvalidOperationException">Neplatná data</exception>
        /// <exception cref="UnauthorizedAccessException">Uživatel nemá oprávnění</exception>
        public async Task UpdateLocation(Location location)
        {
            string userId = await userService.GetUserIdAsync();

            if (location.UserId == userId)
            {
                applicationDbContext.Update(location);
                //Uložení
                await applicationDbContext.SaveChangesAsync();

                //Odpojení všech entit od slednování v EF Core
                applicationDbContext.ChangeTracker.Clear();
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        /// <summary>
        /// Odstranění umístění z databáze
        /// </summary>
        /// <param name="locationId">ID umístění</param>
        /// <returns>Asynchronní operace</returns>
        /// <exception cref="UnauthorizedAccessException">Uživatel nemá oprávnění</exception>
        public async Task DeleteLocationById(int locationId)
        {
            string userId = await userService.GetUserIdAsync();
            Location location = await GetUserLocationById(locationId);

            if (location.UserId == userId)
            {
                applicationDbContext.Remove(location);
                //Uložení
                await applicationDbContext.SaveChangesAsync();

                //Odpojení všech entit od slednování v EF Core
                applicationDbContext.ChangeTracker.Clear();
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }
    }
}
