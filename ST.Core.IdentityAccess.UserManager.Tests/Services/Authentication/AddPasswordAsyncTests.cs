using Microsoft.AspNetCore.Identity;
using System.Collections.Concurrent;
using ST.Core.Identity.Fakes.Models;

namespace ST.Core.IdentityAccess.UserManager.Tests.Services.Authentication
{
    public class InMemoryUserStore :
        IUserStore<TestAppIdentityUser>,
        IUserPasswordStore<TestAppIdentityUser>,
        IUserEmailStore<TestAppIdentityUser>,
        IUserLockoutStore<TestAppIdentityUser>,
        IUserLoginStore<TestAppIdentityUser>
    {
        private readonly ConcurrentDictionary<string, TestAppIdentityUser> _users = new();
        private readonly ConcurrentDictionary<string, string> _normalizedUserNames = new();
        private readonly ConcurrentDictionary<string, string> _normalizedEmails = new();
        private readonly ConcurrentDictionary<string, List<UserLoginInfo>> _logins = new();

        public Task<IdentityResult> CreateAsync(TestAppIdentityUser user, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(user.Id.ToString()))
                throw new InvalidOperationException("Exception: User ID must be set before creation.");

            _users[user.Id.ToString()] = user;
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> DeleteAsync(TestAppIdentityUser user, CancellationToken cancellationToken)
        {
            if (!_users.ContainsKey(user.Id.ToString()))
                return Task.FromResult(Failed("Exception: User not found in store."));

            _users.TryRemove(user.Id.ToString(), out _);
            _normalizedUserNames.TryRemove(user.Id.ToString(), out _);
            _normalizedEmails.TryRemove(user.Id.ToString(), out _);
            _logins.TryRemove(user.Id.ToString(), out _);
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> UpdateAsync(TestAppIdentityUser user, CancellationToken cancellationToken)
        {
            if (!_users.ContainsKey(user.Id.ToString()))
                return Task.FromResult(Failed("Exception: User not found in store."));

            _users[user.Id.ToString()] = user;
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<TestAppIdentityUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
            => Task.FromResult(_users.TryGetValue(userId, out var user) ? user : null);

        public Task<TestAppIdentityUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
            => Task.FromResult(_users.Values.FirstOrDefault(u =>
                _normalizedUserNames.TryGetValue(u.Id.ToString(), out var name) && name == normalizedUserName));

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

        public Task RemoveLoginAsync(TestAppIdentityUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            if (_logins.TryGetValue(user.Id.ToString(), out var logins))
                logins.RemoveAll(l => l.LoginProvider == loginProvider && l.ProviderKey == providerKey);

            return Task.CompletedTask;
        }

        public void Dispose() { }

        // 🔧 Utility methods

        private bool TryEnsureExists(TestAppIdentityUser user)
            => !string.IsNullOrWhiteSpace(user.Id.ToString()) && _users.ContainsKey(user.Id.ToString());

        private void TryUpdate(TestAppIdentityUser user)
        {
            if (!string.IsNullOrWhiteSpace(user.Id.ToString()) && _users.ContainsKey(user.Id.ToString()))
                _users[user.Id.ToString()] = user;
        }

        private IdentityResult Failed(string message)
            => IdentityResult.Failed(new IdentityError { Description = message });
    }
}