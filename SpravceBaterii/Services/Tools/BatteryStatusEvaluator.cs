namespace SpravceBaterii.Services.Tools
{
    public class BatteryStatusEvaluator
    {
        /// <summary>
        /// Výpočet zbývajících dní do konce expirace baterie
        /// </summary>
        /// <param name="expirationDate">Datum expirace baterie</param>
        /// <returns>Počet dní</returns>
        public int RemainingDaysToExpirate(DateOnly expirationDate)
        {
            return (expirationDate.ToDateTime(TimeOnly.MinValue) - DateTime.Today).Days;
        }

        /// <summary>
        /// Výpočet zbývajících dní do předpokládaného data životnosti
        /// </summary>
        /// <param name="insertionDate">Datum vložení baterie do zařízení</param>
        /// <param name="expectedLifespan">Předpokládaný počet dní životnosti</param>
        /// <returns>Počet dní</returns>
        public int RemainingDaysToExpectedLife(DateOnly insertionDate, int expectedLifespan)
        {
            return (insertionDate.AddDays(expectedLifespan).ToDateTime(TimeOnly.MinValue) - DateTime.Today).Days;
        }
    }
}
