using MediatR;
using Microsoft.Extensions.Logging;
using ST.Core.Identity.Application.Authentication.Interfaces;
using ST.Core.Identity.Application.Contracts.Authentication.ApiTokens.Commands;
using ST.Core.Identity.Application.Services;
using ST.Core.Identity.Dtos.Authentication.Logins;
using ST.Core.Identity.Dtos.Authentication.Tokens;

namespace ST.Core.Identity.Application.Authentication.Handlers.ApiTokens.Commands
{
    public class GenerateAuthenticationTokenHandler : IRequestHandler<GenerateAuthenticationTokenCommand, LoginResponseDto>
    {
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;
        private readonly ILogger<GenerateAuthenticationTokenHandler> _logger;

        public GenerateAuthenticationTokenHandler(ITokenService tokenService, IUserService userService, ILogger<GenerateAuthenticationTokenHandler> logger)
        {
            _tokenService = tokenService;
            _userService = userService;
            _logger = logger;
        }

        public async Task<LoginResponseDto> Handle(GenerateAuthenticationTokenCommand request, CancellationToken cancellationToken)
        {
            var user = await _userService.AuthenticateAsync(request.Request.Username, request.Request.Password);
            if (user == null)
            {
                _logger.LogWarning("Authentication failed for user {Username}", request.Request.Username);
                return new LoginResponseDto(false, string.Empty, DateTime.MinValue, Array.Empty<string>(), Guid.Empty, request.Request.Username);
            }

            var token = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken(user);
            var expiresAt = _tokenService.GetAccessTokenExpiry();

            return new LoginResponseDto(true, token, expiresAt, user.Roles, user.Id, user.Username);
        }
    }
}
