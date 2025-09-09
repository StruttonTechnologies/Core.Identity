using MediatR;
using ST.Core.Identity.Application.Contracts.Authentication.Logins.Commands;
using ST.Core.Identity.Dtos.Authentication.Logins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Application.Authentication.Handlers.JwtTokens.Commands
{
    public class InternalLoginCommandHandler : IRequestHandler<InternalLoginCommand, LoginResponseDto>
    {
        private readonly IInternalLoginOrchestrationService _orchestration;

        public InternalLoginCommandHandler(IInternalLoginOrchestrationService orchestration)
        {
            _orchestration = orchestration;
        }

        public async Task<LoginResponseDto> Handle(InternalLoginCommand request, CancellationToken cancellationToken)
        {
            return await _orchestration.AuthenticateAsync(request.LoginRequestDto, cancellationToken);
        }
    }
}
