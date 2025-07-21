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

        public DateOnly InsertionDate { get; set; }

        public int? ExpectedLifespan { get; set; }

        [MaxLength(100)]
        public string? Manufacturer { get; set; }

        public int? DeviceId { get; set; }

        [ForeignKey("DeviceId")]
        public Device? Device { get; set; }

        public int BatteryTypeId { get; set; }

        [ForeignKey("BatteryTypeId")]
        public BatteryType BatteryType { get; set; }

        public int? ChemicalCompositionId { get; set; }

        [ForeignKey("ChemicalCompositionId")]
        public ChemicalComposition? ChemicalComposition { get; set; }

        public bool IsRechargeable { get; set; }

        public int? DisposableBatteryId { get; set; }
        public DisposableBattery? DisposableBattery { get; set; }

        public int? RechargeableBatteryId { get; set; }
        public RechargeableBattery? RechargeableBattery { get; set; }
    }
}
