using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ST.Core.Identity.Dtos.Authentication;

namespace ST.Core.Identity.Dispatch.Authentication.Commands
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
