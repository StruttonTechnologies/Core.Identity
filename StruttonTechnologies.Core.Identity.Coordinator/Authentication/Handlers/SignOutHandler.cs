using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authentication.Commands;
using StruttonTechnologies.Core.Identity.Dtos.Authentication;
using StruttonTechnologies.Core.Identity.Orchestration.Contracts.JwtToken;

namespace StruttonTechnologies.Core.Identity.Coordinator.Authentication.Handlers
{
    public sealed class SignOutHandler<TKey> : IRequestHandler<SignOutCommand, SignOutResultDto>
        where TKey : IEquatable<TKey>
    {
        private readonly ITokenOrchestration<TKey> _tokenOrchestration;

        public SignOutHandler(ITokenOrchestration<TKey> tokenOrchestration)
        {
            _tokenOrchestration = tokenOrchestration
                ?? throw new ArgumentNullException(nameof(tokenOrchestration));
        }

        public async Task<SignOutResultDto> Handle(SignOutCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            string token = request.Token;
            ArgumentException.ThrowIfNullOrWhiteSpace(token);

            await _tokenOrchestration.RevokeRefreshTokenAsync(token, cancellationToken);

            return SignOutResultDto.SuccessResult();
        }
    }
}
