using MediatR;
using ST.Core.Identity.API.Contracts.Authentication;
using ST.Core.Identity.Dtos.Authentication;

namespace ST.Core.Identity.Dispatch.Authentication.Commands
{
    public class RegisterUserCommand : IRequest<RegistrationResultDto>
    {
        public string Email { get; }
        public string Password { get; }
        public string DisplayName { get; }

        public RegisterUserCommand(string email, string password, string displayName)
        {
            Email = email;
            Password = password;
            DisplayName = displayName;
        }
    }
}