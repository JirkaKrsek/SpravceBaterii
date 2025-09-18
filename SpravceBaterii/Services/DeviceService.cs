using Microsoft.EntityFrameworkCore;
using SpravceBaterii.Data;
using SpravceBaterii.Data.Models;

namespace SpravceBaterii.Services
{
    public class DeviceService
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly ApplicationUserService userService;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="applicationDbContext">ApplicationDbContext</param>
        /// <param name="userService">ApplicationUserService</param>
        public DeviceService(ApplicationDbContext applicationDbContext, ApplicationUserService userService)
        {
            this.applicationDbContext = applicationDbContext;
            this.userService = userService;
        }

        /// <summary>
        /// Načtení zařízení přihlášeného uživatele
        /// </summary>
        /// <returns>List zařízení</returns>
        public async Task<List<Device>> GetDevices()
        {
            string userId = await userService.GetUserIdAsync();

            return await applicationDbContext.Devices
                .Where(d => d.UserId == userId)
                .Include(d => d.Location)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Uložení nového zařízení do databáze
        /// </summary>
        /// <param name="device">Zadané zařízení</param>
        /// <returns>Asynchronní operace</returns>
        public async Task AddDevice(Device device)
        {
            string userId = await userService.GetUserIdAsync();
            device.UserId = userId;

            applicationDbContext.Devices.Add(device);
            await applicationDbContext.SaveChangesAsync();
        }
    }
}
