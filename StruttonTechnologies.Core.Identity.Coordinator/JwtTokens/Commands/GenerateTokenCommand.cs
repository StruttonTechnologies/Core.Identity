using MediatR;

using StruttonTechnologies.Core.Identity.Dtos.Authentication;

namespace StruttonTechnologies.Core.Identity.Coordinator.JwtTokens.Commands
{
    /// <summary>
    /// Command to generate JWT access and refresh tokens for a user.
    /// </summary>
    public sealed record GenerateTokenCommand(string UserId) : IRequest<TokenResponseDto>;
}
