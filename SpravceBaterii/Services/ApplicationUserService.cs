using Microsoft.AspNetCore.Components.Authorization;

namespace SpravceBaterii.Services
{
    public class ApplicationUserService
    {
        private readonly AuthenticationStateProvider authenticationStateProvider;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="authenticationStateProvider">AuthenticationStateProvider</param>
        public ApplicationUserService(AuthenticationStateProvider authenticationStateProvider)
        {
            this.authenticationStateProvider = authenticationStateProvider;
        }

        /// <summary>
        /// Získání ID přihlášeného uživatele
        /// </summary>
        /// <returns>string ID uživatele</returns>
        /// <exception cref="UnauthorizedAccessException">Pokud uživatel není přihlášen nebo se nepodaří získat jeho ID</exception>
        public async Task<string> GetUserIdAsync()
        {
            AuthenticationState state = await authenticationStateProvider.GetAuthenticationStateAsync();
            string? userId = state.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException();
            }
            return userId;
        }

        /// <summary>
        /// Zjištění, zda je uživatel přihlášen
        /// </summary>
        /// <returns>bool je/není přihlášen</returns>
        public async Task<bool> IsUserAuthenticatedAsync()
        {
            AuthenticationState state = await authenticationStateProvider.GetAuthenticationStateAsync();
            if (state.User.Identity?.IsAuthenticated == true)
            {
                return true;
            }
            return false;
        }
    }
}
