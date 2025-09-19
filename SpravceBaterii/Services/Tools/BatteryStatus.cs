namespace SpravceBaterii.Services.Tools
{
    public class BatteryStatus
    {
        public int Days { get; }
        public string CssClass { get; } = "battery-noStatus";

        /// <summary>
        /// Konstruktor
        /// </summary>
        public BatteryStatus()
        {
        }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="days">Počet zbývajících dní</param>
        /// <param name="cssClass">Název CSS třídy</param>
        public BatteryStatus(int days, string cssClass)
        {
            Days = days;
            CssClass = cssClass;
        }
    }
}
