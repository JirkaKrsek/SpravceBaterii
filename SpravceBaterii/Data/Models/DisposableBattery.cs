namespace SpravceBaterii.Data.Models
{
    /// <summary>
    /// Modelová třída pro jednorázovou baterii
    /// </summary>
    public class DisposableBattery
    {
        public int Id { get; set; }

        public DateOnly ExpirationDate { get; set; }

        public int BatteryId { get; set; }

        public Battery Battery { get; set; } = null!;
    }
}
