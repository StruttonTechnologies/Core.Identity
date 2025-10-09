using MediatR;
using ST.Core.Identity.Application.Contracts;
using ST.Core.Identity.Dispatch.Authentication.Commands;
using ST.Core.Identity.Dtos.Authentication;

namespace ST.Core.Identity.Dispatch.Authentication.Handlers
{
    public class SignOutHandler : IRequestHandler<SignOutCommand, SignOutResultDto>
    {
        private readonly ITokenService _tokenService;

        public SignOutHandler(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public Task<SignOutResultDto> Handle(SignOutCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _tokenService.RevokeToken(request.Token);
                return Task.FromResult(SignOutResultDto.SuccessResult());
            }
            catch (Exception ex)
            {
                return Task.FromResult(SignOutResultDto.Failure($"Failed to revoke token: {ex.Message}"));
            }
        }
    }
}
