using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpravceBaterii.Data.Models
{
    /// <summary>
    /// Modelová třída pro typ baterie
    /// </summary>
    public class BatteryType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; } = null!;

        public ICollection<Battery>? Batteries { get; set; }
        public ICollection<Device>? Devices { get; set; }
    }
}
