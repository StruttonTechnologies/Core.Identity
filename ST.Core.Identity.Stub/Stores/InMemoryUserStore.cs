using Microsoft.AspNetCore.Identity;
using ST.Core.Identity.Stub.Entities;

namespace ST.Core.Identity.Stub.Stores
{
    public class InMemoryUserStore<TUser> : IUserStore<TUser>
    where TUser : IdentityUser<Guid>, new()
    {
        private readonly Dictionary<Guid, TUser> _users = [];

        public Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken)
        {
            _users[user.Id] = user;
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken)
        {
            _users[user.Id] = user;
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken)
        {
            _users.Remove(user.Id);
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<TUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            if (Guid.TryParse(userId, out var guid) && _users.TryGetValue(guid, out var user))
                return Task.FromResult<TUser?>(user);

            return Task.FromResult<TUser?>(null);
        }

        public Task<TUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var user = _users.Values.FirstOrDefault(u =>
                string.Equals(u.UserName, normalizedUserName, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult<TUser?>(user);
        }

        public Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken)
            => Task.FromResult(user.Id.ToString());

        public Task<string?> GetUserNameAsync(TUser user, CancellationToken cancellationToken)
            => Task.FromResult(user.UserName);

        public Task SetUserNameAsync(TUser user, string? userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public Task<string?> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken)
            => Task.FromResult(user.UserName?.ToUpperInvariant());

        public Task SetNormalizedUserNameAsync(TUser user, string? normalizedName, CancellationToken cancellationToken)
        {
            // no-op for fake
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    } 
}