using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using ST.Core.Identity.Application.Contracts.Authentication.ApiTokens.Queries;
using ST.Core.Identity.Dtos.Authentication.Tokens;
using ST.Core.Identity.Application.Authentication.Interfaces;

namespace ST.Core.Identity.Application.Authentication.ApiTokens.Queries
{
    public class VerifyRefreshTokenHandler : IRequestHandler<VerifyRefreshTokenQuery, bool>
    {
        private readonly ITokenService _tokenService;
        private readonly ILogger<VerifyRefreshTokenHandler> _logger;

        public VerifyRefreshTokenHandler(ITokenService tokenService, ILogger<VerifyRefreshTokenHandler> logger)
        {
            _tokenService = tokenService;
            _logger = logger;
        }

        public Task<bool> Handle(VerifyRefreshTokenQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.RefreshToken))
            {
                _logger.LogWarning("Refresh token is null or empty.");
                return Task.FromResult(false);
            }

            var principal = _tokenService.ValidateToken(request.RefreshToken);
            var isValid = principal != null;

            if (!isValid)
                _logger.LogInformation("Refresh token validation failed.");

            return Task.FromResult(isValid);
        }
    }
}
