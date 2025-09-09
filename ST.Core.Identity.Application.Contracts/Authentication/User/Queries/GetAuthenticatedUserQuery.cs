using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Application.Contracts.Authentication.User.Queries
{
    using ST.Core.Identity.Dtos.Authentication.User;
    using MediatR;

    /// <summary>
    /// Query to retrieve the currently authenticated user's identity details.
    /// </summary>
    public record GetAuthenticatedUserQuery(string UserId) : IRequest<AuthenticatedUserDto>;
}
