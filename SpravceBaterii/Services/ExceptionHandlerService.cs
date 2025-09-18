using Microsoft.AspNetCore.Components;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace SpravceBaterii.Services
{
    public class ExceptionHandlerService
    {
        private readonly NavigationManager navigation;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="navigation">NavigationManager</param>
        public ExceptionHandlerService(NavigationManager navigation)
        {
            this.navigation = navigation;
        }

        /// <summary>
        /// Zpracování přijaté výjimky
        /// Podle typu výjimky se provede vrácení chybové hlášky a případné přesměrování uživatele
        /// </summary>
        /// <param name="exception">Výjimka, která má být zpracována</param>
        /// <returns>Chybová hláška / prázdná chybová hláška + přesměrování na správnou stránku</returns>
        public string HandleException(Exception exception)
        {
            if (exception is UnauthorizedAccessException)
            {
                navigation.NavigateTo("/ucet/prihlaseni", true);
            }
            else if (exception is DbUpdateException or SqlException or TimeoutException)
            {
                return "Databáze není momentálně dostupná. Zkuste to prosím později. Omlouváme se za způsobené komplikace.";
            }
            else
            {
                navigation.NavigateTo("/Error", true);
            }

            return "";
        }
    }
}
