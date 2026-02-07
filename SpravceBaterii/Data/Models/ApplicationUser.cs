using Microsoft.AspNetCore.Identity;

namespace SpravceBaterii.Data.Models
{
    /// <summary>
    /// Modelová třída dědící z IdentityUser ze závislosti Microsoft.Identity
    /// Přidání dalších potřebných atributů
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public DateTime RegistrationDate { get; set; }

        [PersonalData]
        public DateTime LastLoginDate { get; set; }

        public ICollection<Battery>? Batteries { get; set; }
        public ICollection<Device>? Devices { get; set; }
        public ICollection<Location>? Locations { get; set; }
    }
}
