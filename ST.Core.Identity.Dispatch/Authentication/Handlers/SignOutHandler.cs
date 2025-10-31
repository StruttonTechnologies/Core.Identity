using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ST.Core.Identity.Dispatch.Authentication.Commands;
using ST.Core.Identity.Dtos.Authentication;
using ST.Core.Identity.Orchestration.Contracts.JwtToken;

namespace ST.Core.Identity.Dispatch.Authentication.Handlers
{
    public class SignOutHandler<TKey> : IRequestHandler<SignOutCommand, SignOutResultDto>
        where TKey : IEquatable<TKey>
    {
        private readonly ITokenOrchestration<TKey> _tokenService;

        public SignOutHandler(ITokenOrchestration<TKey> TokenOrchestration)
        {
            _tokenService = TokenOrchestration ?? throw new ArgumentNullException(nameof(TokenOrchestration));
        }

        public async Task<SignOutResultDto> Handle(SignOutCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _tokenService.RevokeRefreshTokenAsync(request.Token, cancellationToken);
                return SignOutResultDto.SuccessResult();
            }
            catch (Exception ex)
            {
                return SignOutResultDto.Failure($"Failed to revoke token: {ex.Message}");
            }
        }
    }
}
