using System.Diagnostics.CodeAnalysis;

using MediatR;

using StruttonTechnologies.Core.Identity.Dtos.Authentication;

namespace StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authentication.Commands
{
    [ExcludeFromCodeCoverage]
    public class AuthenticateUserCommand : IRequest<AuthenticationResultDto>
    {
        public string Email { get; }
        public string Password { get; }

        public AuthenticateUserCommand(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
