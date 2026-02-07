using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpravceBaterii.Data.Models
{
    /// <summary>
    /// Modelová třída pro zařízení
    /// </summary>
    public class Device
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Vyplňte název")]
        [StringLength(100, ErrorMessage = "Název může mít max. 100 znaků")]
        public string Name { get; set; } = null!;

        [StringLength(200, ErrorMessage = "Popis může mít max. 200 znaků")]
        public string? Description { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Počet baterií je min. 1")]
        public int BatteryCount { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Vyberte platný typ baterie")]
        public int BatteryTypeId { get; set; }

        public BatteryType BatteryType { get; set; } = null!;

        [StringLength(100, ErrorMessage = "Výrobce může mít max. 100 znaků")]
        public string? Manufacturer { get; set; }

        public int? LocationId { get; set; }

        public Location? Location { get; set; }

        public string UserId { get; set; } = null!;

        public ApplicationUser User { get; set; } = null!;

        public ICollection<Battery>? Batteries { get; set; }
    }
}
