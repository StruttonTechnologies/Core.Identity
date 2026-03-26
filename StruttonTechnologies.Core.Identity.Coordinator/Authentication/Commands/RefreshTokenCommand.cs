using MediatR;

using StruttonTechnologies.Core.Identity.API.Contracts.Authentication;
using StruttonTechnologies.Core.Identity.Dtos.Authentication;

namespace StruttonTechnologies.Core.Identity.Coordinator.Authentication.Commands
{
    public sealed record RefreshTokenCommand(string RefreshToken)
    : IRequest<TokenResponseDto>, IRefreshTokenRequest;
}
