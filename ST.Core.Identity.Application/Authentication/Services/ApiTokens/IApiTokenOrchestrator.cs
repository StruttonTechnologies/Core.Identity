using ST.Core.Identity.Dtos.Authentication.Logins;
using ST.Core.Identity.Dtos.Authentication.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Application.Authentication.Services.ApiTokens
{
    public interface IApiTokenOrchestrator
    {
        Task<LoginResponseDto> AuthenticateAsync(InternalLoginRequestDto request, CancellationToken cancellationToken = default);
        Task<TokenResponseDto> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    }
}
