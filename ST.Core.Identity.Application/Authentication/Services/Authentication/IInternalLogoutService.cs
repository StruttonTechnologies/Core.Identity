using ST.Core.Identity.Domain.Interfaces.Jwtoken;
using ST.Core.Identity.Dtos.Authentication.Logins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Application.Authentication.Services.Authentication
{
    public interface IInternalLogoutService<TKey>
    where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Logs out the user by invalidating tokens and clearing session state.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task representing the logout operation.</returns>
        Task<LogoutResponseDto> LogoutAsync(TKey userId, CancellationToken cancellationToken);
    }
}
