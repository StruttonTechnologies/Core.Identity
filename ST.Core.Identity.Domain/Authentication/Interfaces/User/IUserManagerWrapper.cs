using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Domain.Authentication.Interfaces.User
{
    public interface IUserManagerWrapper<TUser>
    where TUser : IdentityUser
    {
        Task<IdentityResult> AccessFailedAsync(TUser user, CancellationToken cancellationToken = default);
    }
}
