using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.API.Contracts.Authorization
{
    public interface IRemoveClaimRequest
    {
        string UserId { get; }
        string ClaimType { get; }
        string ClaimValue { get; }
    }
}
