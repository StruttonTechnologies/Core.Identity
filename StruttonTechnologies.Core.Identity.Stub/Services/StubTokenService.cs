using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

using StruttonTechnologies.Core.Identity.Orchestration.Contracts.JwtToken;

namespace StruttonTechnologies.Core.Identity.Stub.Services
{
    /// <summary>
    /// Stub implementation of <see cref="ITokenOrchestration{TKey}"/> for unit tests.
    /// Provides deterministic, test-safe behavior.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class StubTokenService : ITokenOrchestration<Guid>
    {
        public Task<string> GenerateTokenAsync(ClaimsPrincipal principal, CancellationToken cancellationToken = default)
        {
            return Task.FromResult("stub-access-token");
        }

        public DateTime GetExpirationTime()
        {
            return DateTime.UtcNow.AddMinutes(15);
        }

        public Task RevokeAccessTokenAsync(string token, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task<bool> IsAccessTokenRevokedAsync(string token, CancellationToken cancellationToken)
        {
            return Task.FromResult(false);
        }

        public Task RevokeRefreshTokenAsync(string token, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task<bool> IsRefreshTokenRevokedAsync(string token, CancellationToken cancellationToken)
        {
            return Task.FromResult(false);
        }

        public Task<ClaimsPrincipal?> ValidateTokenAsync(string token, CancellationToken cancellationToken)
        {
            ClaimsIdentity identity = new ClaimsIdentity(
                new[]
            {
                new Claim(ClaimTypes.NameIdentifier, Guid.Empty.ToString()),
                new Claim(ClaimTypes.Name, "StubUser"),
                new Claim(ClaimTypes.Role, "User")
            }, "Stub");

            return Task.FromResult<ClaimsPrincipal?>(new ClaimsPrincipal(identity));
        }

        public Task<DateTime?> GetExpirationAsync(string token, CancellationToken cancellationToken)
        {
            return Task.FromResult<DateTime?>(DateTime.UtcNow.AddMinutes(15));
        }
    }
}
