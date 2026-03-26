using System.Security.Claims;

using StruttonTechnologies.Core.Identity.Domain.Contracts.Jwtoken;

namespace StruttonTechnologies.Core.Identity.Stub.Managers
{
    internal class StubJwtUserTokenManager : IJwtUserTokenManager<Guid>
    {
        private readonly HashSet<string> _revokedAccessTokens = [];
        private readonly HashSet<string> _revokedRefreshTokens = [];

        public Task<string> GenerateAccessTokenAsync(
            Guid userId,
            string username,
            string email,
            IEnumerable<string> roles,
            CancellationToken cancellationToken)
        {
            string token = $"access-token-for-{username}-{userId}";
            return Task.FromResult(token);
        }

        public Task<string> GenerateRefreshTokenAsync(
            Guid userId,
            string username,
            CancellationToken cancellationToken)
        {
            string token = $"refresh-token-for-{username}-{userId}";
            return Task.FromResult(token);
        }

        public Task<ClaimsPrincipal?> ValidateTokenAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token) || !token.StartsWith("access-token-for-", StringComparison.Ordinal) || _revokedAccessTokens.Contains(token))
            {
                return Task.FromResult<ClaimsPrincipal?>(null);
            }

            ClaimsIdentity identity = new ClaimsIdentity(
                new[]
            {
                new Claim(ClaimTypes.Name, "StubUser"),
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, "TestRole")
            }, "Fake");

            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            return Task.FromResult<ClaimsPrincipal?>(principal);
        }

        public Task<DateTime?> GetExpirationAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return Task.FromResult<DateTime?>(null);
            }

            return Task.FromResult<DateTime?>(DateTime.UtcNow.AddHours(1));
        }

        public Task RevokeAccessTokenAsync(string token, CancellationToken cancellationToken)
        {
            _revokedAccessTokens.Add(token);
            return Task.CompletedTask;
        }

        public Task<bool> IsAccessTokenRevokedAsync(string token, CancellationToken cancellationToken)
        {
            return Task.FromResult(_revokedAccessTokens.Contains(token));
        }

        public Task RevokeRefreshTokenAsync(string token, CancellationToken cancellationToken)
        {
            _revokedRefreshTokens.Add(token);
            return Task.CompletedTask;
        }

        public Task<bool> IsRefreshTokenRevokedAsync(string token, CancellationToken cancellationToken)
        {
            return Task.FromResult(_revokedRefreshTokens.Contains(token));
        }

        public Task RevokeAccessTokensAsync(Guid userId, CancellationToken cancellationToken)
        {
            // Optional: simulate user-wide access token revocation
            return Task.CompletedTask;
        }

        public Task RevokeRefreshTokensAsync(Guid userId, CancellationToken cancellationToken)
        {
            // Optional: simulate user-wide refresh token revocation
            return Task.CompletedTask;
        }
    }
}
