using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpravceBaterii.Data.Models
{
    public class DisposableBattery
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateOnly ExpirationDate { get; set; }

        public int BatteryId { get; set; }

        [ForeignKey("BatteryId")]
        public Battery Battery { get; set; }
    }
}
