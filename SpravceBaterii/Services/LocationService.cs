using Microsoft.EntityFrameworkCore;
using SpravceBaterii.Data;
using SpravceBaterii.Data.Models;
using System.Threading.Tasks;

namespace SpravceBaterii.Services
{
    public class LocationService
    {
        private readonly ApplicationDbContext ApplicationDbContext;
        private readonly ApplicationUserService UserService;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="applicationDbContext">ApplicationDbContext</param>
        /// <param name="userService">ApplicationUserService</param>
        public LocationService(ApplicationDbContext applicationDbContext, ApplicationUserService userService)
        {
            ApplicationDbContext = applicationDbContext;
            UserService = userService;
        }

        /// <summary>
        /// Načtení umístění přihlášeného uživatele
        /// </summary>
        /// <returns>List umístění</returns>
        public async Task<List<Location>> GetLocations()
        {
            string userId = await UserService.GetUserIdAsync();

            return await ApplicationDbContext.Locations
                .Where(l => l.UserId == userId).ToListAsync();
        }

        /// <summary>
        /// Uložené nového umístění do databáze
        /// </summary>
        /// <param name="location">Zadané umístění</param>
        /// <returns>Asynchronní operace</returns>
        public async Task AddLocation(Location location)
        {
            string userId = await UserService.GetUserIdAsync();
            location.UserId = userId;

            ApplicationDbContext.Locations.Add(location);
            await ApplicationDbContext.SaveChangesAsync();

        }
    }
}
