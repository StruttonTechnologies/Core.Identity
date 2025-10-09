using MediatR;
using ST.Core.Identity.Dtos.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Dispatch.Authorization.Queries
{
    /// <summary>
    /// Query to retrieve a ClaimsPrincipalDto for a given user ID.
    /// </summary>
    public sealed record GetClaimsPrincipalQuery(string UserId) : IRequest<ClaimsPrincipalDto>;
}
