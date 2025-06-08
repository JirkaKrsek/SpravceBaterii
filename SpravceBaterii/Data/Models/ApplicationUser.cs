using Microsoft.AspNetCore.Identity;

namespace SpravceBaterii.Data.Models
{
    /// <summary>
    /// Modelová třída dědící z IdentityUser ze závislosti Microsoft.Identity
    /// Přidání dalších potřebných atributů
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

        public DateTime LastLogin { get; set; } = DateTime.UtcNow;

        public ICollection<Location>? Locations { get; set; }
    }
}
