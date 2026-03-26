using MediatR;

using StruttonTechnologies.Core.Identity.Coordinator.Authentication.Commands;
using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authentication;
using StruttonTechnologies.Core.Identity.Dtos.Authentication;

namespace StruttonTechnologies.Core.Identity.Coordinator.Authentication.Dispatcher
{
    public class AuthenticationCoordinator : IAuthenticationCoordinator
    {
        private readonly IMediator _mediator;

        public AuthenticationCoordinator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<RegistrationResultDto> RegisterAsync(string email, string password, string displayName)
        {
            return await _mediator.Send(new RegisterUserCommand(email, password, displayName));
        }

        public async Task<AuthenticationResultDto> AuthenticateAsync(string email, string password)
        {
            return await _mediator.Send(new AuthenticateUserCommand(email, password));
        }

        public async Task<SignOutResultDto> SignOutAsync(string token)
        {
            return await _mediator.Send(new SignOutCommand(token));
        }
    }
}
