using Microsoft.AspNetCore.Identity;

namespace StruttonTechnologies.Core.Identity.Stub.Stores
{
    public class InMemoryUserStore<TUser> : IUserStore<TUser>
    where TUser : IdentityUser<Guid>, new()
    {
        private readonly Dictionary<Guid, TUser> _users = [];

        public Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(user);
            _users[user.Id] = user;
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(user);
            _users[user.Id] = user;
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(user);
            _users.Remove(user.Id);
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<TUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            if (Guid.TryParse(userId, out Guid guid) && _users.TryGetValue(guid, out TUser? user))
            {
                return Task.FromResult<TUser?>(user);
            }

            return Task.FromResult<TUser?>(null);
        }

        public Task<TUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            TUser? user = _users.Values.FirstOrDefault(u =>
                string.Equals(u.UserName, normalizedUserName, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult<TUser?>(user);
        }

        public Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(user);
            return Task.FromResult(user.Id.ToString());
        }

        public Task<string?> GetUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(user);
            return Task.FromResult(user.UserName);
        }

        public Task SetUserNameAsync(TUser user, string? userName, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(user);
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public Task<string?> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(user);
            return Task.FromResult(user.UserName?.ToUpperInvariant());
        }

        public Task SetNormalizedUserNameAsync(TUser user, string? normalizedName, CancellationToken cancellationToken)
        {
            // no-op for fake
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // No managed or unmanaged resources to clean up
        }
    }
}
