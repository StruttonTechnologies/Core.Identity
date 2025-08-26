using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Domain.Authentication.Interfaces.User
{
    public interface IUserRegistration<TUser>
    {
        Task<TUser> CreateAsync(TUser user, string password, CancellationToken cancellationToken = default);
    }
}
