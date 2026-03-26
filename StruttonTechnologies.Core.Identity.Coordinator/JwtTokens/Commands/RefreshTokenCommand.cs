using MediatR;

using StruttonTechnologies.Core.Identity.Dtos.Authentication;

namespace StruttonTechnologies.Core.Identity.Coordinator.JwtTokens.Commands
{
    /// <summary>
    /// Command to refresh JWT tokens using a refresh token.
    /// </summary>
    public sealed record RefreshTokenCommand(string RefreshToken) : IRequest<TokenResponseDto>;
}