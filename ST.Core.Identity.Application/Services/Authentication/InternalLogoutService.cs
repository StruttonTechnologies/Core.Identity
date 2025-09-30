using ST.Core.Identity.Domain.Interfaces.Jwtoken;
using ST.Core.Identity.Dtos.Authentication.Logins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Application.Services.Authentication
{
    public class InternalLogoutService<TKey> : IInternalLogoutService<TKey>
     where TKey : IEquatable<TKey>
    {
        private readonly IJwtUserTokenManager<TKey> _tokenManager;

        public InternalLogoutService(IJwtUserTokenManager<TKey> tokenManager)
        {
            _tokenManager = tokenManager;
        }

        public async Task<LogoutResponseDto> LogoutAsync(TKey userId, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(userId);

            // Invalidate refresh tokens
            await _tokenManager.RevokeRefreshTokensAsync(userId, cancellationToken);


            // Return confirmation
            return new LogoutResponseDto(
                LoggedOutAt: DateTime.UtcNow,
                Message: "User successfully logged out."
            );
        }
    }
}
