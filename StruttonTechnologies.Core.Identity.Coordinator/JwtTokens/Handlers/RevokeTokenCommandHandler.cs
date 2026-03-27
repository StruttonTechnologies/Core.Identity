using StruttonTechnologies.Core.Identity.Coordinator.Contracts.JwtTokens.Commands;
using StruttonTechnologies.Core.Identity.Domain.Contracts.JwtToken;

namespace StruttonTechnologies.Core.Identity.Coordinator.JwtTokens.Handlers
{
    /// <summary>
    /// MediatR handler that processes requests to revoke JWT tokens.
    /// </summary>
    /// <typeparam name="TUser">The type representing a user in the system, must inherit from <see cref="IdentityUser{TKey}"/>.</typeparam>
    /// <typeparam name="TKey">The type used for user keys, must implement <see cref="IEquatable{TKey}"/>.</typeparam>
    internal class RevokeTokenCommandHandler<TUser, TKey>
        : IRequestHandler<RevokeTokenCommand, Unit>
        where TUser : IdentityUser<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        private readonly UserManager<TUser> _userManager;
        private readonly IJwtUserTokenManager<TKey> _tokenManager;

        public RevokeTokenCommandHandler(
            UserManager<TUser> userManager,
            IJwtUserTokenManager<TKey> tokenManager)
        {
            _userManager = userManager
                ?? throw new ArgumentNullException(nameof(userManager));
            _tokenManager = tokenManager
                ?? throw new ArgumentNullException(nameof(tokenManager));
        }

        public async Task<Unit> Handle(
            RevokeTokenCommand request,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            // If revoking by token string
            if (!string.IsNullOrEmpty(request.Token))
            {
                if (request.IsRefreshToken)
                {
                    await _tokenManager.RevokeRefreshTokenAsync(request.Token, cancellationToken);
                }
                else
                {
                    await _tokenManager.RevokeAccessTokenAsync(request.Token, cancellationToken);
                }
            }

            // If revoking all tokens for a user
            else if (!string.IsNullOrEmpty(request.UserId))
            {
                TUser? user = await _userManager.FindByIdAsync(request.UserId);

                if (user == null)
                {
                    throw new InvalidOperationException($"User with ID '{request.UserId}' not found.");
                }

                await _tokenManager.RevokeAccessTokensAsync(user.Id, cancellationToken);
                await _tokenManager.RevokeRefreshTokensAsync(user.Id, cancellationToken);
            }
            else
            {
                throw new InvalidOperationException("Either Token or UserId must be provided.");
            }

            return Unit.Value;
        }
    }
}
