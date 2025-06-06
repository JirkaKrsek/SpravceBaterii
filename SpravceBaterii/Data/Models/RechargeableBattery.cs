using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpravceBaterii.Data.Models
{
    /// <summary>
    /// Modelová třída pro nabíjecí baterii
    /// </summary>
    public class RechargeableBattery
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? Capacity { get; set; }

        public int? CycleCount { get; set; }

        public int BatteryId { get; set; }

        [ForeignKey("BatteryId")]
        public Battery Battery { get; set; }

        public ICollection<ChargingHistory>? ChargingHistories { get; set; }
    }
}
