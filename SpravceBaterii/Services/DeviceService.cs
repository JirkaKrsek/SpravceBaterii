using Microsoft.EntityFrameworkCore;
using SpravceBaterii.Data;
using SpravceBaterii.Data.Models;
using System.Threading.Tasks;

namespace SpravceBaterii.Services
{
    public class DeviceService
    {
        private readonly ApplicationDbContext ApplicationDbContext;
        private readonly ApplicationUserService UserService;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="applicationDbContext">ApplicationDbContext</param>
        /// <param name="userService">ApplicationUserService</param>
        public DeviceService(ApplicationDbContext applicationDbContext, ApplicationUserService userService)
        {
            ApplicationDbContext = applicationDbContext;
            UserService = userService;
        }

        /// <summary>
        /// Načtení zařízení přihlášeného uživatele
        /// </summary>
        /// <returns>List zařízení</returns>
        public async Task<List<Device>> GetDevices()
        {
            string userId = await UserService.GetUserIdAsync();

            return await ApplicationDbContext.Devices
                .Where(d => d.UserId == userId).ToListAsync();
        }

        /// <summary>
        /// Uložení nového zařízení do databáze
        /// </summary>
        /// <param name="device">Zadané zařízení</param>
        /// <returns>Asynchronní operace</returns>
        public async Task AddDevice(Device device)
        {
            string userId = await UserService.GetUserIdAsync();
            device.UserId = userId;

            ApplicationDbContext.Devices.Add(device);
            await ApplicationDbContext.SaveChangesAsync();
        }
    }
}
