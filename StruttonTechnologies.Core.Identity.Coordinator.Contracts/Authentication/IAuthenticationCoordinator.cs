using StruttonTechnologies.Core.Identity.Dtos.Authentication;

namespace StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authentication
{
    /// <summary>
    /// Defines the authentication coordination contract for registration, sign-in, and sign-out flows.
    /// </summary>
    public interface IAuthenticationCoordinator
    {
        /// <summary>Registers a new user.</summary>
        public Task<RegistrationResultDto> RegisterAsync(string email, string password, string displayName);

        /// <summary>Authenticates an existing user.</summary>
        public Task<AuthenticationResultDto> AuthenticateAsync(string email, string password);

        /// <summary>Signs the current user out.</summary>
        public Task<SignOutResultDto> SignOutAsync(string token);
    }
}
