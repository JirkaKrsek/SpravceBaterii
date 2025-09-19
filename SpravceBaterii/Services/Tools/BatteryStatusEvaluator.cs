namespace SpravceBaterii.Services.Tools
{
    public class BatteryStatusEvaluator
    {
        /// <summary>
        /// Výpočet zbývajících dní do konce expirace baterie
        /// </summary>
        /// <param name="expirationDate">Datum expirace baterie</param>
        /// <returns>Stav baterie</returns>
        public BatteryStatus RemainingDaysToExpirate(DateOnly expirationDate)
        {
            int remainingDays = (expirationDate.ToDateTime(TimeOnly.MinValue) - DateTime.Today).Days;
            string cssClass = GetCssClass(remainingDays);

            return new BatteryStatus(remainingDays, cssClass);
        }

        /// <summary>
        /// Výpočet zbývajících dní do předpokládaného data životnosti
        /// </summary>
        /// <param name="insertionDate">Datum vložení baterie do zařízení</param>
        /// <param name="expectedLifespan">Předpokládaný počet dní životnosti</param>
        /// <returns>Stav baterie</returns>
        public BatteryStatus RemainingDaysToExpectedLife(DateOnly insertionDate, int expectedLifespan)
        {
            int remainingDays = (insertionDate.AddDays(expectedLifespan).ToDateTime(TimeOnly.MinValue) - DateTime.Today).Days;
            string cssClass = GetCssClass(remainingDays);

            return new BatteryStatus(remainingDays, cssClass);
        }

        /// <summary>
        /// Vyhodnocení stavu baterie podle vypočtených zbývajících dní pro stylování
        /// </summary>
        /// <param name="days">Počet zbývajících dní</param>
        /// <returns>Název CSS třídy</returns>
        private static string GetCssClass(int days)
        {
            if (days <= 0)
            {
                return "battery-expired";
            }
            else if (days <= 30)
            {
                return "battery-warning";
            }
            return "battery-ok";
        }
    }
}
