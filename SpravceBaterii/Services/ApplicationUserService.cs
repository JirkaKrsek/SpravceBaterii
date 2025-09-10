using Microsoft.AspNetCore.Components.Authorization;

namespace SpravceBaterii.Services
{
    public class ApplicationUserService
    {
        private readonly AuthenticationStateProvider AuthenticationStateProvider;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="authenticationStateProvider">AuthenticationStateProvider</param>
        public ApplicationUserService(AuthenticationStateProvider authenticationStateProvider)
        {
            AuthenticationStateProvider = authenticationStateProvider;
        }

        /// <summary>
        /// Získání ID přihlášeného uživatele
        /// </summary>
        /// <returns>string? ID</returns>
        public async Task<string?> GetUserIdAsync()
        {
            AuthenticationState state = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            return state.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
