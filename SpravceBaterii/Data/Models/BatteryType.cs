using System.ComponentModel.DataAnnotations;

namespace SpravceBaterii.Data.Models
{
    /// <summary>
    /// Modelová třída pro typ baterie
    /// </summary>
    public class BatteryType
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; } = null!;

        public ICollection<Battery>? Batteries { get; set; }
        public ICollection<Device>? Devices { get; set; }
    }
}
