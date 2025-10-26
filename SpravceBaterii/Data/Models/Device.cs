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

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(200)]
        public string? Description { get; set; }

        public int BatteryCount { get; set; }

        public int BatteryTypeId { get; set; }

        [ForeignKey("BatteryTypeId")]
        public BatteryType BatteryType { get; set; }

        [MaxLength(100)]
        public string? Manufacturer { get; set; }

        public int? LocationId { get; set; }

        [ForeignKey("LocationId")]
        public Location? Location { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public ICollection<Battery>? Batteries { get; set; }
    }
}
