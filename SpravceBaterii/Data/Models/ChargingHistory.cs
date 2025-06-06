using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpravceBaterii.Data.Models
{
    /// <summary>
    /// Modelová třída pro historii nabíjení baterie
    /// </summary>
    public class ChargingHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateOnly ChargeDate { get; set; }

        public int? CapacityAfterCharge { get; set; }

        public int RechargeableBatteryId { get; set; }

        [ForeignKey("RechargeableBatteryId")]
        public RechargeableBattery RechargeableBattery { get; set; }
    }
}
