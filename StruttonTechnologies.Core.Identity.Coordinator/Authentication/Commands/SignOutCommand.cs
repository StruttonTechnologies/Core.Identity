using MediatR;

using StruttonTechnologies.Core.Identity.Dtos.Authentication;

namespace StruttonTechnologies.Core.Identity.Coordinator.Authentication.Commands
{
    public class SignOutCommand : IRequest<SignOutResultDto>
    {
        public string Token { get; }

        public SignOutCommand(string token)
        {
            Token = token;
        }
    }
}
