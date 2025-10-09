using ST.Core.Identity.Dtos.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Dispatcher.Contracts.Authorization
{
    public interface IAuthorizationDispatcher
    {
        Task<ClaimsPrincipalDto?> GetClaimsPrincipalAsync(string userId);
    }
}
