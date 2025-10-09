using ST.Core.Identity.Domain.Interfaces.Jwtoken;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace ST.Core.IdentityAccess.Fakes.JwtToken
{
    /// <summary>
    /// Fake implementation of <see cref="IJwtUserTokenManager{TKey}"/> for testing.
    /// Returns deterministic tokens and stubbed responses without real crypto or persistence.
    /// </summary>
    public class TestTokenManager : IJwtUserTokenManager<Guid>
    {
        public Task<string> GenerateAccessTokenAsync(
            Guid userId,
            string username,
            string email,
            IEnumerable<string> roles,
            CancellationToken cancellationToken)
        {
            var token = $"access-token-for-{username}-{userId}";
            return Task.FromResult(token);
        }

        public Task<string> GenerateRefreshTokenAsync(
            Guid userId,
            string username,
            CancellationToken cancellationToken)
        {
            var token = $"refresh-token-for-{username}-{userId}";
            return Task.FromResult(token);
        }

        public Task<ClaimsPrincipal?> ValidateTokenAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token) || !token.StartsWith("access-token-for-"))
                return Task.FromResult<ClaimsPrincipal?>(null);

            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "StubUser"),
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, "TestRole")
            }, "Fake");

            var principal = new ClaimsPrincipal(identity);
            return Task.FromResult<ClaimsPrincipal?>(principal);
        }

        public Task<DateTime?> GetExpirationAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return Task.FromResult<DateTime?>(null);

            return Task.FromResult<DateTime?>(DateTime.UtcNow.AddHours(1));
        }

        public Task RevokeRefreshTokensAsync(Guid userId, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task RevokeRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task RevokeAccessTokensAsync(Guid userId, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}