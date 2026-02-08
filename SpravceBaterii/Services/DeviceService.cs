using Microsoft.EntityFrameworkCore;
using SpravceBaterii.Data;
using SpravceBaterii.Data.Models;
using System;

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
                .Include(d => d.Batteries!)
                    .ThenInclude(b => b.DisposableBattery)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Načtení zařízení podle ID umístění
        /// </summary>
        /// <returns>List zařízení</returns>
        public async Task<List<Device>> GetUserDevicesByLocationId(int locationId)
        {
            string userId = await userService.GetUserIdAsync();

            return await applicationDbContext.Devices
                .Where(d => d.UserId == userId && d.LocationId == locationId)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Zjištění, zda umístění obsahuje zařízení
        /// </summary>
        /// <param name="locationId">ID umístění</param>
        /// <returns>bool</returns>
        public async Task<bool> AnyDevicesInLocationById(int locationId)
        {
            string userId = await userService.GetUserIdAsync();

            return await applicationDbContext.Devices
                .AnyAsync(d => d.UserId == userId && d.LocationId == locationId);
        }

        /// <summary>
        /// Získání zařízení podle ID
        /// </summary>
        /// <param name="batteryId">ID hledaného zařízení</param>
        /// <returns>Nalezené zařízení</returns>
        /// <exception cref="KeyNotFoundException">Zařízení nenalezeno</exception>
        public async Task<Device> GetUserDeviceById(int deviceId)
        {
            string userId = await userService.GetUserIdAsync();

            return await applicationDbContext.Devices
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.UserId == userId && d.Id == deviceId)
                ?? throw new KeyNotFoundException();
        }

        /// <summary>
        /// Získání zařízení podle ID
        /// </summary>
        /// <param name="deviceId">ID hledaného zařízení</param>
        /// <returns>Nalezené zařízení</returns>
        /// <exception cref="KeyNotFoundException">Zařízení nenalezeno</exception>
        public async Task<Device> GetUserDeviceByIdWithBatteries(int deviceId)
        {
            string userId = await userService.GetUserIdAsync();

            return await applicationDbContext.Devices
                .Include(d => d.Batteries)
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.UserId == userId && d.Id == deviceId)
                ?? throw new KeyNotFoundException();
        }

        /// <summary>
        /// Získání zařízení s jeho detaily podle ID
        /// </summary>
        /// <param name="deviceId">ID hledaného zařízení</param>
        /// <returns>Nalezené zařízení</returns>
        /// <exception cref="KeyNotFoundException">Zařízení nenalezeno</exception>
        public async Task<Device> GetUserDeviceByIdWithDetails(int deviceId)
        {
            string userId = await userService.GetUserIdAsync();

            return await applicationDbContext.Devices
                .Include(d => d.Batteries)
                .Include(d => d.BatteryType)
                .Include(d => d.Location)
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.UserId == userId && d.Id == deviceId)
                ?? throw new KeyNotFoundException();
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

            //Odpojení od slednování EF Core
            applicationDbContext.Entry(device).State = EntityState.Detached;
        }

        /// <summary>
        /// Aktualizace zařízení v databázi
        /// </summary>
        /// <param name="battery">Upravené zařízení</param>
        /// <returns>Asynchronní operace</returns>
        /// <exception cref="InvalidOperationException">Neplatná data</exception>
        /// <exception cref="UnauthorizedAccessException">Uživatel nemá oprávnění</exception>
        public async Task UpdateDevice(Device device)
        {
            string userId = await userService.GetUserIdAsync();

            if (device.UserId == userId)
            {
                applicationDbContext.Update(device);
                //Uložení
                await applicationDbContext.SaveChangesAsync();

                //Odpojení od slednování EF Core
                applicationDbContext.Entry(device).State = EntityState.Detached;
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        /// <summary>
        /// Odpojení všech zařízení z daného umístění
        /// </summary>
        /// <param name="locationId">ID umístění</param>
        /// <returns>Asynchronní operace</returns>
        public async Task UnassignDevicesInLocationById(int locationId)
        {
            List<Device> devices = await GetUserDevicesByLocationId(locationId);

            foreach (Device device in devices)
            {
                device.LocationId = null;
            }

            applicationDbContext.UpdateRange(devices);
            //Uložení
            await applicationDbContext.SaveChangesAsync();

            foreach (Device device in devices)
            {
                //Odpojení od slednování EF Core
                applicationDbContext.Entry(device).State = EntityState.Detached;
            }
        }

        /// <summary>
        /// Odstranění zařízení z databáze
        /// </summary>
        /// <param name="batteryId">ID zařízení</param>
        /// <returns>Asynchronní operace</returns>
        /// <exception cref="UnauthorizedAccessException">Uživatel nemá oprávnění</exception>
        public async Task DeleteDeviceById(int deviceId)
        {
            string userId = await userService.GetUserIdAsync();
            Device device = await GetUserDeviceById(deviceId);

            if (device.UserId == userId)
            {
                applicationDbContext.Remove(device);
                //Uložení
                await applicationDbContext.SaveChangesAsync();
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }
    }
}
