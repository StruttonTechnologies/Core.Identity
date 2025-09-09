using ST.Core.Identity.Domain.Authentication.Interfaces.Jwtoken;
using ST.Core.Identity.Domain.Authentication.Interfaces.Token;
using ST.Core.Identity.Domain.Authorization.Entities;
using System.Security.Claims;
using System.Security.Cryptography;

namespace ST.Core.Identity.Infrastructure.Authentication.Token
{
    public class JwtUserTokenManager : IJwtUserTokenManager
    {
        private readonly IJwtUserTokenManager _jwtTokenService;
        private readonly IRefreshTokenStore _refreshTokenStore;

        public JwtUserTokenManager(IJwtUserTokenManager jwtTokenService, IRefreshTokenStore refreshTokenStore)
        {
            _jwtTokenService = jwtTokenService;
            _refreshTokenStore = refreshTokenStore;
        }

        public async Task<string> GenerateAccessTokenAsync(string userId, string username, string email, IEnumerable<string> roles, CancellationToken cancellationToken)
        {
            var identity = new ClaimsIdentity("Identity.Application");

            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId));
            identity.AddClaim(new Claim(ClaimTypes.Name, username));
            identity.AddClaim(new Claim(ClaimTypes.Email, email ?? string.Empty));

            foreach (var role in roles)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            var principal = new ClaimsPrincipal(identity);
            return await _jwtTokenService.GenerateTokenAsync(principal);
        }

        public async Task<string> GenerateRefreshTokenAsync(string userId, string username, CancellationToken cancellationToken)
        {
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

            var refreshToken = new RefreshToken
            {
                Token = token,
                UserId = userId,
                Username = username,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(30),
                IsRevoked = false
            };

            await _refreshTokenStore.SaveAsync(refreshToken, cancellationToken);
            return token;
        }

        public Task<ClaimsPrincipal?> ValidateTokenAsync(string token)
            => _jwtTokenService.ValidateTokenAsync(token);

        public Task<DateTime?> GetExpirationAsync(string token)
            => _jwtTokenService.GetExpirationAsync(token);
    }
}