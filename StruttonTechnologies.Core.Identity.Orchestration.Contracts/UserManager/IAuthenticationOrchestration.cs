using StruttonTechnologies.Core.Identity.Dtos.Authentication;

namespace StruttonTechnologies.Core.Identity.Orchestration.Contracts.UserManager
{
    /// <summary>
    /// Contract for orchestrating user authentication workflows.
    /// Validates credentials using ASP.NET Core Identity and issues JWT tokens.
    /// </summary>
    /// <typeparam name="TKey">
    /// The type of the unique identifier for the user, typically <c>Guid</c> or <c>string</c>.
    /// </typeparam>
    public interface IAuthenticationOrchestration<TKey>
        where TKey : IEquatable<TKey>
    {
        public Task<AuthenticationResultDto> AuthenticateAsync(string email, string password, CancellationToken ct);
        public Task<AuthenticationResultDto> RegisterAsync(string email, string password, CancellationToken ct);
        public Task SignOutAsync(string accessToken, CancellationToken ct);
    }
}
