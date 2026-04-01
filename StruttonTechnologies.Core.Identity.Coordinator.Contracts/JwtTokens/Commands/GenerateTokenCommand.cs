using System.Diagnostics.CodeAnalysis;

using MediatR;

using StruttonTechnologies.Core.Identity.Dtos.Authentication;

namespace StruttonTechnologies.Core.Identity.Coordinator.Contracts.JwtTokens.Commands
{
    /// <summary>
    /// Command to generate JWT access and refresh tokens for a user.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed record GenerateTokenCommand(string UserId) : IRequest<TokenResponseDto>;
}
