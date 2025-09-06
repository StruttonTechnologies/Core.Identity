using MediatR;
using ST.Core.Identity.Dtos.Authentication.User;

namespace ST.Core.Identity.Application.Contracts.Authentication.User.Queries
{
    public record GetEmailQuery(UpdateEmailRequestDto Dto) : IRequest<UserResponseDto>;
}