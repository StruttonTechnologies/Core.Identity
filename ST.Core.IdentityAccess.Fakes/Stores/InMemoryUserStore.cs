using Microsoft.AspNetCore.Identity;
using System.Collections.Concurrent;
using ST.Core.Identity.Stub.Entities;

namespace ST.Core.IdentityAccess.Fakes.Stores
{
    public class InMemoryUserStore :
        IUserStore<StubUser>,
        IUserPasswordStore<StubUser>,
        IUserEmailStore<StubUser>,
        IUserLockoutStore<StubUser>,
        IUserLoginStore<StubUser>,
        IUserTwoFactorStore<StubUser>,
        IUserPhoneNumberStore<StubUser>,
        IUserRoleStore<StubUser>

    {
        private readonly ConcurrentDictionary<string, StubUser> _users = new();
        private readonly ConcurrentDictionary<string, string> _normalizedUserNames = new();
        private readonly ConcurrentDictionary<string, string> _normalizedEmails = new();
        private readonly ConcurrentDictionary<string, List<UserLoginInfo>> _logins = new();
        private readonly HashSet<string> _deletedUserIds = [];
        private readonly ConcurrentDictionary<string, List<string>> _userRoles = new();


        public Task<IdentityResult> CreateAsync(StubUser user, CancellationToken cancellationToken)
        {
            string userId = user.Id.ToString();

            if (string.IsNullOrWhiteSpace(userId))
                throw new InvalidOperationException("Exception: User ID must be set before creation.");

            if (_users.ContainsKey(userId) || _deletedUserIds.Contains(userId))
                return Task.FromResult(Failed("Exception: User already exists or was previously deleted."));

            _users[userId] = user;
            return Task.FromResult(IdentityResult.Success);
        }


        public Task<IdentityResult> DeleteAsync(StubUser user, CancellationToken cancellationToken)
        {
            string userId = user.Id.ToString();

            if (!_users.ContainsKey(userId))
                return Task.FromResult(Failed("Exception: User not found in store."));

            _users.TryRemove(userId, out _);
            _normalizedUserNames.TryRemove(userId, out _);
            _normalizedEmails.TryRemove(userId, out _);
            _logins.TryRemove(userId, out _);
            _deletedUserIds.Add(userId);
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> UpdateAsync(StubUser user, CancellationToken cancellationToken)
        {
            string userId = user.Id.ToString();

            if (!_users.ContainsKey(userId))
                return Task.FromResult(Failed("Exception: User not found in store."));

            _users[userId] = user;
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<StubUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            if (_deletedUserIds.Contains(userId))
                return Task.FromResult<StubUser?>(null);

            return Task.FromResult(_users.TryGetValue(userId, out var user) ? user : null);
        }

        public Task<StubUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            if (normalizedUserName.Equals("simulate-exception", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Simulated failure for testing.");

            return Task.FromResult(_users.Values.FirstOrDefault(u =>
                _normalizedUserNames.TryGetValue(u.Id.ToString(), out var name) && name == normalizedUserName));
        }

        public Task<string> GetUserIdAsync(StubUser user, CancellationToken cancellationToken)
            => Task.FromResult(user.Id.ToString());


        public Task<string?> GetUserNameAsync(StubUser user, CancellationToken cancellationToken)
            => Task.FromResult(user.UserName);

 
        public Task SetUserNameAsync(StubUser user, string? userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            TryUpdate(user);
            return Task.CompletedTask;
        }

        public Task<string?> GetNormalizedUserNameAsync(StubUser user, CancellationToken cancellationToken)
            => Task.FromResult(_normalizedUserNames.TryGetValue(user.Id.ToString(), out var name) ? name : user.UserName?.ToUpperInvariant());


        public Task SetNormalizedUserNameAsync(StubUser user, string? normalizedName, CancellationToken cancellationToken)
        {
            _normalizedUserNames[user.Id.ToString()] = normalizedName ?? string.Empty;
            return Task.CompletedTask;
        }

        public Task<string?> GetPasswordHashAsync(StubUser user, CancellationToken cancellationToken)
            => Task.FromResult(user.PasswordHash);


        public Task<bool> HasPasswordAsync(StubUser user, CancellationToken cancellationToken)
            => Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));

        public Task SetPasswordHashAsync(StubUser user, string? passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            TryUpdate(user);
            return Task.CompletedTask;
        }


        public Task<string?> GetEmailAsync(StubUser user, CancellationToken cancellationToken)
            => Task.FromResult(user.Email);


        public Task<bool> GetEmailConfirmedAsync(StubUser user, CancellationToken cancellationToken)
            => Task.FromResult(user.EmailConfirmed);

        public Task SetEmailAsync(StubUser user, string? email, CancellationToken cancellationToken)
        {
            user.Email = email;
            TryUpdate(user);
            return Task.CompletedTask;
        }

        public Task SetEmailConfirmedAsync(StubUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;
            TryUpdate(user);
            return Task.CompletedTask;
        }

        public Task<StubUser?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
            => Task.FromResult(_users.Values.FirstOrDefault(u =>
                _normalizedEmails.TryGetValue(u.Id.ToString(), out var email) && email == normalizedEmail));

        public Task<string?> GetNormalizedEmailAsync(StubUser user, CancellationToken cancellationToken)
            => Task.FromResult(_normalizedEmails.TryGetValue(user.Id.ToString(), out var email) ? email : user.Email?.ToUpperInvariant());

        public Task SetNormalizedEmailAsync(StubUser user, string? normalizedEmail, CancellationToken cancellationToken)
        {
            _normalizedEmails[user.Id.ToString()] = normalizedEmail ?? string.Empty;
            return Task.CompletedTask;
        }

        public Task<int> GetAccessFailedCountAsync(StubUser user, CancellationToken cancellationToken)
            => Task.FromResult(user.AccessFailedCount);

        public Task<int> IncrementAccessFailedCountAsync(StubUser user, CancellationToken cancellationToken)
        {
            if (!TryEnsureExists(user))
                throw new InvalidOperationException("Exception: User not found in store.");

            user.AccessFailedCount++;
            _users[user.Id.ToString()] = user;
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task ResetAccessFailedCountAsync(StubUser user, CancellationToken cancellationToken)
        {
            if (!TryEnsureExists(user))
                throw new InvalidOperationException("Exception: User not found in store.");

            user.AccessFailedCount = 0;
            _users[user.Id.ToString()] = user;
            return Task.CompletedTask;
        }

        public Task<bool> GetLockoutEnabledAsync(StubUser user, CancellationToken cancellationToken)
            => Task.FromResult(user.LockoutEnabled);

        public Task SetLockoutEnabledAsync(StubUser user, bool enabled, CancellationToken cancellationToken)
        {
            user.LockoutEnabled = enabled;
            TryUpdate(user);
            return Task.CompletedTask;
        }

        public Task<DateTimeOffset?> GetLockoutEndDateAsync(StubUser user, CancellationToken cancellationToken)
            => Task.FromResult(user.LockoutEnd);

        public Task SetLockoutEndDateAsync(StubUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
        {
            user.LockoutEnd = lockoutEnd;
            TryUpdate(user);
            return Task.CompletedTask;
        }

        public Task AddLoginAsync(StubUser user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            if (!_logins.ContainsKey(user.Id.ToString()))
                _logins[user.Id.ToString()] = new List<UserLoginInfo>();

            _logins[user.Id.ToString()].Add(login);
            return Task.CompletedTask;
        }

        public Task<StubUser?> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            var userId = _logins.FirstOrDefault(kvp =>
                kvp.Value.Any(l => l.LoginProvider == loginProvider && l.ProviderKey == providerKey)).Key;

            return Task.FromResult(userId != null && _users.TryGetValue(userId, out var user) ? user : null);
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(StubUser user, CancellationToken cancellationToken)
            => Task.FromResult<IList<UserLoginInfo>>(_logins.TryGetValue(user.Id.ToString(), out var logins) ? logins : new List<UserLoginInfo>());

        public Task<bool> GetTwoFactorEnabledAsync(StubUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.TwoFactorEnabled);
        }

        public Task SetTwoFactorEnabledAsync(StubUser user, bool enabled, CancellationToken cancellationToken)
        {
            user.TwoFactorEnabled = enabled;
            return Task.CompletedTask;
        }

        public Task RemoveLoginAsync(StubUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            if (_logins.TryGetValue(user.Id.ToString(), out var logins))
                logins.RemoveAll(l => l.LoginProvider == loginProvider && l.ProviderKey == providerKey);

            return Task.CompletedTask;
        }

        public Task<string?> GetPhoneNumberAsync(StubUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        public Task SetPhoneNumberAsync(StubUser user, string? phoneNumber, CancellationToken cancellationToken)
        {
            user.PhoneNumber = phoneNumber;
            return Task.CompletedTask;
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(StubUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public Task SetPhoneNumberConfirmedAsync(StubUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.PhoneNumberConfirmed = confirmed;
            return Task.CompletedTask;
        }

        public void Dispose() { }

        private bool TryEnsureExists(StubUser user)
            => !string.IsNullOrWhiteSpace(user.Id.ToString()) && _users.ContainsKey(user.Id.ToString());

        private void TryUpdate(StubUser user)
        {
            if (!string.IsNullOrWhiteSpace(user.Id.ToString()) && _users.ContainsKey(user.Id.ToString()))
            {
                _users[user.Id.ToString()] = user;
            }
        }

        private static IdentityResult Failed(string message)
        {
            return IdentityResult.Failed(new IdentityError { Description = message });
        }

        public Task AddToRoleAsync(StubUser user, string roleName, CancellationToken cancellationToken)
        {
            var key = user.Id.ToString();
            if (!_userRoles.ContainsKey(key))
                _userRoles[key] = new List<string>();

            _userRoles[key].Add(roleName);
            return Task.CompletedTask;
        }

        public Task RemoveFromRoleAsync(StubUser user, string roleName, CancellationToken cancellationToken)
        {
            var key = user.Id.ToString();
            _userRoles[key]?.Remove(roleName);
            return Task.CompletedTask;
        }

        public Task<IList<string>> GetRolesAsync(StubUser user, CancellationToken cancellationToken)
        {
            var key = user.Id.ToString();
            var roles = _userRoles.TryGetValue(key, out var list) ? list : new List<string>();
            return Task.FromResult((IList<string>)roles);
        }

        public Task<bool> IsInRoleAsync(StubUser user, string roleName, CancellationToken cancellationToken)
        {
            var key = user.Id.ToString();
            var isInRole = _userRoles.TryGetValue(key, out var list) && list.Contains(roleName);
            return Task.FromResult(isInRole);
        }

        public Task<IList<StubUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            var usersInRole = _userRoles
                .Where(kvp => kvp.Value.Contains(roleName))
                .Select(kvp => _users.Values.FirstOrDefault(u => u.Id.ToString() == kvp.Key))
                .Where(u => u != null)
                .ToList();

            return Task.FromResult((IList<StubUser>)usersInRole);
        }

    }
}