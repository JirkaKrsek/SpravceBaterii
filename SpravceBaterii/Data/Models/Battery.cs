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

        [StringLength(100, ErrorMessage = "Výrobce může mít max. 100 znaků")]
        public string? Manufacturer { get; set; }

        [StringLength(200, ErrorMessage = "Historie použití může mít max. 200 znaků")]
        public string? UsageHistory { get; set; }

        [StringLength(200, ErrorMessage = "Popis může mít max. 200 znaků")]
        public string? Description { get; set; }

        public string UserId { get; set; } = null!;

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; } = null!;

        public int? DeviceId { get; set; }

        [ForeignKey("DeviceId")]
        public Device? Device { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Vyberte platný typ baterie")]
        public int BatteryTypeId { get; set; }

        [ForeignKey("BatteryTypeId")]
        public BatteryType BatteryType { get; set; } = null!;

        public int? ChemicalCompositionId { get; set; }

        [ForeignKey("ChemicalCompositionId")]
        public ChemicalComposition? ChemicalComposition { get; set; }

        public bool IsRechargeable { get; set; }

        public DisposableBattery? DisposableBattery { get; set; }

        public RechargeableBattery? RechargeableBattery { get; set; }
    }
}
