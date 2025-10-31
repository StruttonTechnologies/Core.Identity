using MediatR;
using ST.Core.Identity.Dispatch.Authentication.Commands;
using ST.Core.Identity.Dtos.Authentication;
using ST.Core.Identity.Orchestration.Contracts;
using ST.Core.Identity.Orchestration.Contracts.UserManager;

namespace ST.Core.Identity.Dispatch.Authentication.Handlers
{
    /// <summary>
    /// MediatR handler that delegates user authentication to the orchestration service.
    /// </summary>
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
            return _authOrchestration.AuthenticateAsync(
                request.Email,
                request.Password,
                cancellationToken);
        }
    }
}