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
                .AsNoTracking()
                .Where(l => l.UserId == userId).ToListAsync();
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

            //Odpojení od slednování EF Core
            applicationDbContext.Entry(location).State = EntityState.Detached;
        }
    }
}
