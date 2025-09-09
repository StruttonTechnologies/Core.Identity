using MediatR;
using ST.Core.Identity.Dtos.Authentication.Logins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Application.Contracts.Authentication.Logins.Commands
{
    /// <summary>
    /// Command to authenticate a user via an external login provider.
    /// </summary>
    public record ExternalLoginCommand(ExternalLoginRequestDto Request) : IRequest<LoginResponseDto>;
}
