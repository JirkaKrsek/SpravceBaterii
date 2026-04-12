using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SpravceBaterii.Data.Models
{
    /// <summary>
    /// Modelová třída pro nabíjecí baterii
    /// </summary>
    public class RechargeableBattery
    {
        public int Id { get; set; }

        [Precision(18, 2)]
        [Range(0.1, double.MaxValue, ErrorMessage = "Kapacita baterie je min. 0.1")]
        public decimal? Capacity { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Počet cyklů baterie je min. 1")]
        public int? CycleCount { get; set; }

        public int BatteryId { get; set; }

        public Battery Battery { get; set; } = null!;
    }
}
