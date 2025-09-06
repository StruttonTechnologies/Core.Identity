using MediatR;
using ST.Core.Identity.Dtos.Authentication.Password;

namespace ST.Core.Identity.Application.Contracts.Authentication.Password.Commands
{
    public record ResetPasswordCommand(ResetPasswordRequestDto Dto) : IRequest<PasswordStatusResponseDto>;
}