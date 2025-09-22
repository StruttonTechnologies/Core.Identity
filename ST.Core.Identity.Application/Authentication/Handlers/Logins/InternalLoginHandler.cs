using MediatR;
using Microsoft.AspNetCore.Identity;
using ST.Core.Identity.Application.Authentication.Abstractions.Orchistration.Login;
using ST.Core.Identity.Application.Contracts.Authentication.Logins.Commands;
using ST.Core.Identity.Dtos.Authentication.Logins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Application.Authentication.Handlers.Logins
{
    public class InternalLoginHandler<TUser, TKey> : IRequestHandler<InternalLoginCommand, LoginResponseDto>
    where TUser : IdentityUser<TKey>, new()
    where TKey : IEquatable<TKey>
    {
        private readonly IInternalLoginService<TUser, TKey> _loginService;

        public InternalLoginHandler(IInternalLoginService<TUser, TKey> loginService)
        {
            _loginService = loginService;
        }

        public async Task<LoginResponseDto> Handle(InternalLoginCommand command, CancellationToken cancellationToken)
        {
            return await _loginService.AuthenticateAsync(command.Request, cancellationToken);
        }
    }
}
