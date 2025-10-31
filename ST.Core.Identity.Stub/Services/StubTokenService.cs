using ST.Core.Identity.Orchestration.Contracts.JwtToken;
using System.Security.Claims;

namespace ST.Core.Identity.Stub.Services
{
    /// <summary>
    /// Stub implementation of <see cref="ITokenOrchestration{TKey}"/> for unit tests.
    /// Provides deterministic, test-safe behavior.
    /// </summary>
    public class StubTokenService : ITokenOrchestration<Guid>
    {
        public Task<string> GenerateTokenAsync(ClaimsPrincipal principal, CancellationToken cancellationToken = default)
            => Task.FromResult("stub-access-token");

        public DateTime GetExpirationTime()
            => DateTime.UtcNow.AddMinutes(15);

        public Task RevokeAccessTokenAsync(string token, CancellationToken cancellationToken)
            => Task.CompletedTask;

        public Task<bool> IsAccessTokenRevokedAsync(string token, CancellationToken cancellationToken)
            => Task.FromResult(false);

        public Task RevokeRefreshTokenAsync(string token, CancellationToken cancellationToken)
            => Task.CompletedTask;

        public Task<bool> IsRefreshTokenRevokedAsync(string token, CancellationToken cancellationToken)
            => Task.FromResult(false);

        public Task<ClaimsPrincipal?> ValidateTokenAsync(string token, CancellationToken cancellationToken)
        {
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, Guid.Empty.ToString()),
                new Claim(ClaimTypes.Name, "StubUser"),
                new Claim(ClaimTypes.Role, "User")
            }, "Stub");

            return Task.FromResult<ClaimsPrincipal?>(new ClaimsPrincipal(identity));
        }

        public Task<DateTime?> GetExpirationAsync(string token, CancellationToken cancellationToken)
            => Task.FromResult<DateTime?>(DateTime.UtcNow.AddMinutes(15));
    }
}