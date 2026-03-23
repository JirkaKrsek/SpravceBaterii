using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using SpravceBaterii.Data;
using SpravceBaterii.Data.Models;

namespace SpravceBaterii.Services
{
    public class ApplicationUserService
    {
        private readonly AuthenticationStateProvider authenticationStateProvider;
        private readonly ApplicationDbContext applicationDbContext;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="authenticationStateProvider">AuthenticationStateProvider</param>
        /// <param name="applicationDbContext">ApplicationDbContext</param>
        public ApplicationUserService(AuthenticationStateProvider authenticationStateProvider, ApplicationDbContext applicationDbContext)
        {
            this.authenticationStateProvider = authenticationStateProvider;
            this.applicationDbContext = applicationDbContext;
        }

        /// <summary>
        /// Získání ID přihlášeného uživatele
        /// </summary>
        /// <returns>string ID uživatele</returns>
        /// <exception cref="UnauthorizedAccessException">Pokud uživatel není přihlášen nebo se nepodaří získat jeho ID</exception>
        public async Task<string> GetUserIdAsync()
        {
            AuthenticationState state = await authenticationStateProvider.GetAuthenticationStateAsync();
            string? userId = state.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException();
            }
            return userId;
        }

        /// <summary>
        /// Zjištění, zda je uživatel přihlášen
        /// </summary>
        /// <returns>bool je/není přihlášen</returns>
        public async Task<bool> IsUserAuthenticatedAsync()
        {
            AuthenticationState state = await authenticationStateProvider.GetAuthenticationStateAsync();
            if (state.User.Identity?.IsAuthenticated == true)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Získání informací aktuálně přihlášeného uživatele
        /// </summary>
        /// <returns>Nalezený uživatel</returns>
        /// <exception cref="KeyNotFoundException">Uživatel nenalezen</exception>
        public async Task<ApplicationUser> GetUserInformations()
        {
            string userId = await GetUserIdAsync();

            return await applicationDbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == userId)
            ?? throw new KeyNotFoundException();
        }

        /// <summary>
        /// Výpočet všech uložených záznamů přihlášeného uživatele
        /// </summary>
        /// <returns>UserStatisticsDto</returns>
        public async Task<UserStatisticsDto> GetUserStatistics()
        {
            string userId = await GetUserIdAsync();

            int disposableBatteries = await applicationDbContext.Batteries
                .CountAsync(b => b.UserId == userId && !b.IsRechargeable);

            int rechargeableBatteries = await applicationDbContext.Batteries
                .CountAsync(b => b.UserId == userId && b.IsRechargeable);

            int batteries = disposableBatteries + rechargeableBatteries;

            int devices = await applicationDbContext.Devices
                .CountAsync(d => d.UserId == userId);

            int locations = await applicationDbContext.Locations
                .CountAsync(l => l.UserId == userId);

            int totalCount = disposableBatteries + rechargeableBatteries + devices + locations;

            return new UserStatisticsDto(
                totalCount,
                batteries,
                disposableBatteries,
                rechargeableBatteries,
                devices,
                locations
            );
        }


        /// <summary>
        /// Smazání všech uložených dat aktuálně přihlášeného uživatele
        /// </summary>
        /// <returns>Asynchronní operace</returns>
        public async Task DeleteAllUserData()
        {
            string userId = await GetUserIdAsync();

            List<Battery> batteries = await applicationDbContext.Batteries
                .Where(b => b.UserId == userId)
                .AsNoTracking()
                .ToListAsync();
            applicationDbContext.RemoveRange(batteries);

            List<Device> devices = await applicationDbContext.Devices
                .Where(b => b.UserId == userId)
                .AsNoTracking()
                .ToListAsync();
            applicationDbContext.RemoveRange(devices);

            List<Location> locations = await applicationDbContext.Locations
                .Where(b => b.UserId == userId)
                .AsNoTracking()
                .ToListAsync();
            applicationDbContext.RemoveRange(locations);

            //Uložení
            await applicationDbContext.SaveChangesAsync();

            //Odpojení všech entit od slednování v EF Core
            applicationDbContext.ChangeTracker.Clear();
        }
    }

    public record UserStatisticsDto(int TotalCount, int Batteries, int DisposableBatteries, int RechargeableBatteries, int Devices, int Locations);
}
