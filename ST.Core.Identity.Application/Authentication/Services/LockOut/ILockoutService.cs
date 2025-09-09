using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Domain.Authentication.Interfaces.UserManager
{
    public interface ILockoutService
    {
        Task<bool> IsLockedOutAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<DateTimeOffset?> GetLockoutEndDateAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<int> GetAccessFailedCountAsync(Guid userId, CancellationToken cancellationToken = default);
        Task SetLockoutEndDateAsync(Guid userId, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken = default);
        Task SetLockoutEnabledAsync(Guid userId, bool enabled, CancellationToken cancellationToken = default);
        Task ResetAccessFailedCountAsync(Guid userId, CancellationToken cancellationToken = default);
        Task AccessFailedAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
