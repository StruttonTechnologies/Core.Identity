using ST.Core.Identity.Dtos.Authentication;

namespace ST.Core.Identity.Orchestration.Contracts.UserManager
{
    /// <summary>
    /// Contract for orchestrating user authentication workflows.
    /// Validates credentials using ASP.NET Core Identity and issues JWT tokens.
    /// </summary>
    public interface IAuthenticationOrchestration<TKey>
        where TKey : IEquatable<TKey>
    {
        
        Task<AuthenticationResultDto> AuthenticateAsync(string email, string password, CancellationToken ct);
        Task<AuthenticationResultDto> RegisterAsync(string email, string password, CancellationToken ct);
        Task SignOutAsync(string accessToken, CancellationToken ct);
    }
}