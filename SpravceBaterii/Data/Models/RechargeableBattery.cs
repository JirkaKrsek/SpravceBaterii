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

        [Range(1, int.MaxValue, ErrorMessage = "Kapacita baterie je min. 1")]
        public int? Capacity { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Počet cyklů baterie je min. 1")]
        public int? CycleCount { get; set; }

        public int BatteryId { get; set; }

        [ForeignKey("BatteryId")]
        public Battery Battery { get; set; }

        public ICollection<ChargingHistory>? ChargingHistories { get; set; }
    }
}
