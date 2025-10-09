using MediatR;
using Microsoft.AspNetCore.Identity;
using ST.Core.Identity.API.Contracts.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Dispatch.Authentication.Commands
{
    public sealed record ResetPasswordCommand(string UserId, string Token, string NewPassword)
    : IRequest<IdentityResult>, IResetPasswordRequest;
}
