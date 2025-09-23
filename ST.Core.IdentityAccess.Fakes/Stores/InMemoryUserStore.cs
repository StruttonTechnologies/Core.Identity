using Microsoft.AspNetCore.Identity;
using System.Collections.Concurrent;
using ST.Core.Identity.Fakes.Models;

namespace ST.Core.IdentityAccess.Fakes.Stores
{
    public class InMemoryUserStore :
        IUserStore<TestAppIdentityUser>,
        IUserPasswordStore<TestAppIdentityUser>,
        IUserEmailStore<TestAppIdentityUser>,
        IUserLockoutStore<TestAppIdentityUser>,
        IUserLoginStore<TestAppIdentityUser>,
        IUserTwoFactorStore<TestAppIdentityUser>,
        IUserPhoneNumberStore<TestAppIdentityUser>,
        IUserRoleStore<TestAppIdentityUser>

    {
        private readonly ConcurrentDictionary<string, TestAppIdentityUser> _users = new();
        private readonly ConcurrentDictionary<string, string> _normalizedUserNames = new();
        private readonly ConcurrentDictionary<string, string> _normalizedEmails = new();
        private readonly ConcurrentDictionary<string, List<UserLoginInfo>> _logins = new();
        private readonly HashSet<string> _deletedUserIds = new();
        private readonly ConcurrentDictionary<string, List<string>> _userRoles = new();


        public Task<IdentityResult> CreateAsync(TestAppIdentityUser user, CancellationToken cancellationToken)
        {
            string userId = user.Id.ToString();

            if (string.IsNullOrWhiteSpace(userId))
                throw new InvalidOperationException("Exception: User ID must be set before creation.");

            if (_users.ContainsKey(userId) || _deletedUserIds.Contains(userId))
                return Task.FromResult(Failed("Exception: User already exists or was previously deleted."));

            _users[userId] = user;
            return Task.FromResult(IdentityResult.Success);
        }


        public Task<IdentityResult> DeleteAsync(TestAppIdentityUser user, CancellationToken cancellationToken)
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

        public Task<IdentityResult> UpdateAsync(TestAppIdentityUser user, CancellationToken cancellationToken)
        {
            string userId = user.Id.ToString();

            if (!_users.ContainsKey(userId))
                return Task.FromResult(Failed("Exception: User not found in store."));

            _users[userId] = user;
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<TestAppIdentityUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            if (_deletedUserIds.Contains(userId))
                return Task.FromResult<TestAppIdentityUser?>(null);

            return Task.FromResult(_users.TryGetValue(userId, out var user) ? user : null);
        }

        public Task<TestAppIdentityUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            if (normalizedUserName.Equals("simulate-exception", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Simulated failure for testing.");

            return Task.FromResult(_users.Values.FirstOrDefault(u =>
                _normalizedUserNames.TryGetValue(u.Id.ToString(), out var name) && name == normalizedUserName));
        }

        public Task<string> GetUserIdAsync(TestAppIdentityUser user, CancellationToken cancellationToken)
            => Task.FromResult(user.Id.ToString());


        public Task<string> GetUserNameAsync(TestAppIdentityUser user, CancellationToken cancellationToken)
            => Task.FromResult(user.UserName);

 
        public Task SetUserNameAsync(TestAppIdentityUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            TryUpdate(user);
            return Task.CompletedTask;
        }

        public Task<string> GetNormalizedUserNameAsync(TestAppIdentityUser user, CancellationToken cancellationToken)
            => Task.FromResult(_normalizedUserNames.TryGetValue(user.Id.ToString(), out var name) ? name : user.UserName?.ToUpperInvariant() ?? string.Empty);


        public Task SetNormalizedUserNameAsync(TestAppIdentityUser user, string normalizedName, CancellationToken cancellationToken)
        {
            _normalizedUserNames[user.Id.ToString()] = normalizedName;
            return Task.CompletedTask;
        }

        public Task<string?> GetPasswordHashAsync(TestAppIdentityUser user, CancellationToken cancellationToken)
            => Task.FromResult(user.PasswordHash);


        public Task<bool> HasPasswordAsync(TestAppIdentityUser user, CancellationToken cancellationToken)
            => Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));

        public Task SetPasswordHashAsync(TestAppIdentityUser user, string? passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            TryUpdate(user);
            return Task.CompletedTask;
        }


        public Task<string?> GetEmailAsync(TestAppIdentityUser user, CancellationToken cancellationToken)
            => Task.FromResult(user.Email);


        public Task<bool> GetEmailConfirmedAsync(TestAppIdentityUser user, CancellationToken cancellationToken)
            => Task.FromResult(user.EmailConfirmed);

        public Task SetEmailAsync(TestAppIdentityUser user, string? email, CancellationToken cancellationToken)
        {
            user.Email = email;
            TryUpdate(user);
            return Task.CompletedTask;
        }

        public Task SetEmailConfirmedAsync(TestAppIdentityUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;
            TryUpdate(user);
            return Task.CompletedTask;
        }

        public Task<TestAppIdentityUser?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
            => Task.FromResult(_users.Values.FirstOrDefault(u =>
                _normalizedEmails.TryGetValue(u.Id.ToString(), out var email) && email == normalizedEmail));

        public Task<string> GetNormalizedEmailAsync(TestAppIdentityUser user, CancellationToken cancellationToken)
            => Task.FromResult(_normalizedEmails.TryGetValue(user.Id.ToString(), out var email) ? email : user.Email?.ToUpperInvariant() ?? string.Empty);

        public Task SetNormalizedEmailAsync(TestAppIdentityUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            _normalizedEmails[user.Id.ToString()] = normalizedEmail;
            return Task.CompletedTask;
        }

        public Task<int> GetAccessFailedCountAsync(TestAppIdentityUser user, CancellationToken cancellationToken)
            => Task.FromResult(user.AccessFailedCount);

        public Task<int> IncrementAccessFailedCountAsync(TestAppIdentityUser user, CancellationToken cancellationToken)
        {
            if (!TryEnsureExists(user))
                throw new InvalidOperationException("Exception: User not found in store.");

            user.AccessFailedCount++;
            _users[user.Id.ToString()] = user;
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task ResetAccessFailedCountAsync(TestAppIdentityUser user, CancellationToken cancellationToken)
        {
            if (!TryEnsureExists(user))
                throw new InvalidOperationException("Exception: User not found in store.");

            user.AccessFailedCount = 0;
            _users[user.Id.ToString()] = user;
            return Task.CompletedTask;
        }

        public Task<bool> GetLockoutEnabledAsync(TestAppIdentityUser user, CancellationToken cancellationToken)
            => Task.FromResult(user.LockoutEnabled);

        public Task SetLockoutEnabledAsync(TestAppIdentityUser user, bool enabled, CancellationToken cancellationToken)
        {
            user.LockoutEnabled = enabled;
            TryUpdate(user);
            return Task.CompletedTask;
        }

        public Task<DateTimeOffset?> GetLockoutEndDateAsync(TestAppIdentityUser user, CancellationToken cancellationToken)
            => Task.FromResult(user.LockoutEnd);

        public Task SetLockoutEndDateAsync(TestAppIdentityUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
        {
            user.LockoutEnd = lockoutEnd;
            TryUpdate(user);
            return Task.CompletedTask;
        }

        public Task AddLoginAsync(TestAppIdentityUser user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            if (!_logins.ContainsKey(user.Id.ToString()))
                _logins[user.Id.ToString()] = new List<UserLoginInfo>();

            _logins[user.Id.ToString()].Add(login);
            return Task.CompletedTask;
        }

        public Task<TestAppIdentityUser?> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            var userId = _logins.FirstOrDefault(kvp =>
                kvp.Value.Any(l => l.LoginProvider == loginProvider && l.ProviderKey == providerKey)).Key;

            return Task.FromResult(userId != null && _users.TryGetValue(userId, out var user) ? user : null);
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(TestAppIdentityUser user, CancellationToken cancellationToken)
            => Task.FromResult<IList<UserLoginInfo>>(_logins.TryGetValue(user.Id.ToString(), out var logins) ? logins : new List<UserLoginInfo>());

        public Task<bool> GetTwoFactorEnabledAsync(TestAppIdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.TwoFactorEnabled);
        }

        public Task SetTwoFactorEnabledAsync(TestAppIdentityUser user, bool enabled, CancellationToken cancellationToken)
        {
            user.TwoFactorEnabled = enabled;
            return Task.CompletedTask;
        }

        public Task RemoveLoginAsync(TestAppIdentityUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            if (_logins.TryGetValue(user.Id.ToString(), out var logins))
                logins.RemoveAll(l => l.LoginProvider == loginProvider && l.ProviderKey == providerKey);

            return Task.CompletedTask;
        }

        public Task<string> GetPhoneNumberAsync(TestAppIdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        public Task SetPhoneNumberAsync(TestAppIdentityUser user, string phoneNumber, CancellationToken cancellationToken)
        {
            user.PhoneNumber = phoneNumber;
            return Task.CompletedTask;
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(TestAppIdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public Task SetPhoneNumberConfirmedAsync(TestAppIdentityUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.PhoneNumberConfirmed = confirmed;
            return Task.CompletedTask;
        }

        public void Dispose() { }

        private bool TryEnsureExists(TestAppIdentityUser user)
            => !string.IsNullOrWhiteSpace(user.Id.ToString()) && _users.ContainsKey(user.Id.ToString());

        private void TryUpdate(TestAppIdentityUser user)
        {
            if (!string.IsNullOrWhiteSpace(user.Id.ToString()) && _users.ContainsKey(user.Id.ToString()))
            {
                _users[user.Id.ToString()] = user;
            }
        }

        private IdentityResult Failed(string message)
            => IdentityResult.Failed(new IdentityError { Description = message });

        public Task AddToRoleAsync(TestAppIdentityUser user, string roleName, CancellationToken cancellationToken)
        {
            var key = user.Id.ToString();
            if (!_userRoles.ContainsKey(key))
                _userRoles[key] = new List<string>();

            _userRoles[key].Add(roleName);
            return Task.CompletedTask;
        }

        public Task RemoveFromRoleAsync(TestAppIdentityUser user, string roleName, CancellationToken cancellationToken)
        {
            var key = user.Id.ToString();
            _userRoles[key]?.Remove(roleName);
            return Task.CompletedTask;
        }

        public Task<IList<string>> GetRolesAsync(TestAppIdentityUser user, CancellationToken cancellationToken)
        {
            var key = user.Id.ToString();
            var roles = _userRoles.TryGetValue(key, out var list) ? list : new List<string>();
            return Task.FromResult((IList<string>)roles);
        }

        public Task<bool> IsInRoleAsync(TestAppIdentityUser user, string roleName, CancellationToken cancellationToken)
        {
            var key = user.Id.ToString();
            var isInRole = _userRoles.TryGetValue(key, out var list) && list.Contains(roleName);
            return Task.FromResult(isInRole);
        }

        public Task<IList<TestAppIdentityUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            var usersInRole = _userRoles
                .Where(kvp => kvp.Value.Contains(roleName))
                .Select(kvp => _users.Values.FirstOrDefault(u => u.Id.ToString() == kvp.Key))
                .Where(u => u != null)
                .ToList();

            return Task.FromResult((IList<TestAppIdentityUser>)usersInRole);
        }

    }
}