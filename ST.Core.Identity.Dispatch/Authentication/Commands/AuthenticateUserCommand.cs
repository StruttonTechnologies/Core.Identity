using MediatR;
using ST.Core.Identity.Dtos.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Dispatch.Authentication.Commands
{
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
