using Microsoft.AspNetCore.Identity;
using ST.Core.Identity.Stub.Entities;
using System.Collections.Concurrent;

namespace ST.Core.Identity.Stub.Stores
{
    /// <summary>
    /// In-memory stub implementation of IUserStore for testing Identity flows.
    /// </summary>
    public sealed class StubUserStore :
        IUserStore<StubUser>,
        IUserPasswordStore<StubUser>,
        IUserRoleStore<StubUser>
    {
        private readonly ConcurrentDictionary<Guid, StubUser> _users = new();

        public Task<IdentityResult> CreateAsync(StubUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.Id = Guid.NewGuid();
            _users[user.Id] = user;
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> UpdateAsync(StubUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _users[user.Id] = user;
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> DeleteAsync(StubUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _users.TryRemove(user.Id, out _);
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<StubUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Guid.TryParse(userId, out var id) && _users.TryGetValue(id, out var user)
                ? Task.FromResult<StubUser?>(user)
                : Task.FromResult<StubUser?>(null);
        }

        public Task<StubUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var user = _users.Values.FirstOrDefault(u => u.NormalizedUserName == normalizedUserName);
            return Task.FromResult(user);
        }

        public Task<string> GetUserIdAsync(StubUser user, CancellationToken cancellationToken)
            => Task.FromResult(user.Id.ToString());

        public Task<string?> GetUserNameAsync(StubUser user, CancellationToken cancellationToken)
            => Task.FromResult(user.UserName);

        public Task SetUserNameAsync(StubUser user, string? userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public Task<string?> GetNormalizedUserNameAsync(StubUser user, CancellationToken cancellationToken)
            => Task.FromResult(user.NormalizedUserName);

        public Task SetNormalizedUserNameAsync(StubUser user, string? normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        // --- Password store methods ---
        public Task SetPasswordHashAsync(StubUser user, string? passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task<string?> GetPasswordHashAsync(StubUser user, CancellationToken cancellationToken)
            => Task.FromResult(user.PasswordHash);

        public Task<bool> HasPasswordAsync(StubUser user, CancellationToken cancellationToken)
            => Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));

        // --- Role store methods ---
        public Task AddToRoleAsync(StubUser user, string roleName, CancellationToken cancellationToken)
        {
            user.Roles.Add(roleName);
            return Task.CompletedTask;
        }

        public Task RemoveFromRoleAsync(StubUser user, string roleName, CancellationToken cancellationToken)
        {
            user.Roles.Remove(roleName);
            return Task.CompletedTask;
        }

        public Task<IList<string>> GetRolesAsync(StubUser user, CancellationToken cancellationToken)
            => Task.FromResult((IList<string>)user.Roles.ToList());

        public Task<bool> IsInRoleAsync(StubUser user, string roleName, CancellationToken cancellationToken)
            => Task.FromResult(user.Roles.Contains(roleName));

        public Task<IList<StubUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            var users = _users.Values.Where(u => u.Roles.Contains(roleName)).ToList();
            return Task.FromResult((IList<StubUser>)users);
        }

        public void Dispose()
        {
            // No unmanaged resources to release
        }
    }
}