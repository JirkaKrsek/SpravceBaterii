using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpravceBaterii.Data.Models
{
    /// <summary>
    /// Modelová třída pro baterii
    /// </summary>
    public class Battery
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Počet baterií je min. 1")]
        public int Count { get; set; }

        public DateOnly? InsertionDate { get; set; }

        public int? ExpectedLifespan { get; set; }

        [MaxLength(100)]
        public string? Manufacturer { get; set; }

        [StringLength(200)]
        public string? UsageHistory { get; set; }

        [StringLength(200)]
        public string? Description { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public int? DeviceId { get; set; }

        [ForeignKey("DeviceId")]
        public Device? Device { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Vyberte platný typ baterie")]
        public int BatteryTypeId { get; set; }

        [ForeignKey("BatteryTypeId")]
        public BatteryType BatteryType { get; set; }

        public int? ChemicalCompositionId { get; set; }

        [ForeignKey("ChemicalCompositionId")]
        public ChemicalComposition? ChemicalComposition { get; set; }

        public bool IsRechargeable { get; set; }

        public DisposableBattery? DisposableBattery { get; set; }

        public RechargeableBattery? RechargeableBattery { get; set; }
    }
}
