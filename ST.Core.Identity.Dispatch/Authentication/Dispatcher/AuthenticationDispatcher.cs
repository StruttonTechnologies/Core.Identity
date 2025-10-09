using MediatR;
using ST.Core.Identity.Dispatch.Authentication.Commands;
using ST.Core.Identity.Dispatcher.Contracts.Authentication;
using ST.Core.Identity.Dtos.Authentication;

namespace ST.Core.Identity.Dispatch.Authentication.Dispatcher
{
   

    public class AuthenticationDispatcher: IAuthenticationDispatcher
    {
        private readonly IMediator _mediator;

        public AuthenticationDispatcher(IMediator mediator)
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
