using MediatR;

using StruttonTechnologies.Core.Identity.Dtos.Authentication;

namespace StruttonTechnologies.Core.Identity.Coordinator.Authentication.Commands
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
