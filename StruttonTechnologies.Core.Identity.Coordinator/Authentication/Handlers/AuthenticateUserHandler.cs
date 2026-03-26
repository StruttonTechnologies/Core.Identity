using MediatR;

using StruttonTechnologies.Core.Identity.Coordinator.Authentication.Commands;
using StruttonTechnologies.Core.Identity.Dtos.Authentication;
using StruttonTechnologies.Core.Identity.Orchestration.Contracts.UserManager;

namespace StruttonTechnologies.Core.Identity.Coordinator.Authentication.Handlers
{
    /// <summary>
    /// MediatR handler that delegates user authentication to the orchestration service.
    /// </summary>
    /// <typeparam name="TKey">The type used for user keys, must implement <see cref="IEquatable{TKey}"/>.</typeparam>
    public class AuthenticateUserHandler<TKey>
    : IRequestHandler<AuthenticateUserCommand, AuthenticationResultDto>
    where TKey : IEquatable<TKey>
    {
        private readonly IAuthenticationOrchestration<TKey> _authOrchestration;

        public AuthenticateUserHandler(IAuthenticationOrchestration<TKey> authOrchestration)
        {
            _authOrchestration = authOrchestration
                ?? throw new ArgumentNullException(nameof(authOrchestration));
        }

        public Task<AuthenticationResultDto> Handle(
            AuthenticateUserCommand request,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            return _authOrchestration.AuthenticateAsync(
                request.Email,
                request.Password,
                cancellationToken);
        }
    }
}
