using Microsoft.AspNetCore.Identity;
using System.Collections.Concurrent;
using ST.Core.Identity.Fakes.Models;
namespace ST.Core.IdentityAccess.Fakes.Stores
{
    /// <summary>
    /// In-memory implementation of <see cref="IUserStore{TUser}"/> and <see cref="IUserLockoutStore{TUser}"/>
    /// for <see cref="TestAppIdentityUser"/>. Used for testing purposes.
    /// </summary>
    public class InMemoryUserStore :
        IUserStore<TestAppIdentityUser>,
        IUserLockoutStore<TestAppIdentityUser>
    {
        /// <summary>
        /// Stores users by their ID.
        /// </summary>
        private readonly ConcurrentDictionary<string, TestAppIdentityUser> _users = new();

        /// <summary>
        /// Creates a new user in the store.
        /// </summary>
        /// <param name="user">The user to create.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>An <see cref="IdentityResult"/> indicating success.</returns>
        /// <exception cref="InvalidOperationException">Thrown if user ID is not set.</exception>
        public Task<IdentityResult> CreateAsync(TestAppIdentityUser user, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(user.Id))
                throw new InvalidOperationException("User ID must be set before creation.");

            _users[user.Id] = user;
            return Task.FromResult(IdentityResult.Success);
        }

        /// <summary>
        /// Finds a user by their ID.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The user if found; otherwise, null.</returns>
        public Task<TestAppIdentityUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            _users.TryGetValue(userId, out var user);
            return Task.FromResult(user);
        }

        /// <summary>
        /// Deletes a user from the store.
        /// </summary>
        /// <param name="user">The user to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>An <see cref="IdentityResult"/> indicating success.</returns>
        public Task<IdentityResult> DeleteAsync(TestAppIdentityUser user, CancellationToken cancellationToken)
        {
            _users.TryRemove(user.Id, out _);
            return Task.FromResult(IdentityResult.Success);
        }

        /// <summary>
        /// Updates a user in the store.
        /// </summary>
        /// <param name="user">The user to update.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>An <see cref="IdentityResult"/> indicating success.</returns>
        public Task<IdentityResult> UpdateAsync(TestAppIdentityUser user, CancellationToken cancellationToken)
        {
            _users[user.Id] = user;
            return Task.FromResult(IdentityResult.Success);
        }

        /// <summary>
        /// Finds a user by their normalized username.
        /// </summary>
        /// <param name="normalizedUserName">The normalized username.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The user if found; otherwise, null.</returns>
        public Task<TestAppIdentityUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var user = _users.Values.FirstOrDefault(u => u.UserName.ToUpperInvariant() == normalizedUserName);
            return Task.FromResult(user);
        }

        /// <summary>
        /// Gets the user ID for the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The user ID.</returns>
        public Task<string> GetUserIdAsync(TestAppIdentityUser user, CancellationToken cancellationToken)
            => Task.FromResult(user.Id);

        /// <summary>
        /// Gets the username for the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The username.</returns>
        public Task<string> GetUserNameAsync(TestAppIdentityUser user, CancellationToken cancellationToken)
            => Task.FromResult(user.UserName);

        /// <summary>
        /// Sets the username for the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="userName">The username to set.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A completed task.</returns>
        public Task SetUserNameAsync(TestAppIdentityUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Gets the normalized username for the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The normalized username.</returns>
        public Task<string> GetNormalizedUserNameAsync(TestAppIdentityUser user, CancellationToken cancellationToken)
            => Task.FromResult(user.UserName.ToUpperInvariant());

        /// <summary>
        /// Sets the normalized username for the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="normalizedName">The normalized username to set.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A completed task.</returns>
        public Task SetNormalizedUserNameAsync(TestAppIdentityUser user, string normalizedName, CancellationToken cancellationToken)
            => Task.CompletedTask;

        /// <summary>
        /// Gets the access failed count for the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The access failed count.</returns>
        public Task<int> GetAccessFailedCountAsync(TestAppIdentityUser user, CancellationToken cancellationToken)
            => Task.FromResult(user.AccessFailedCount);

        /// <summary>
        /// Increments the access failed count for the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The new access failed count.</returns>
        public Task<int> IncrementAccessFailedCountAsync(TestAppIdentityUser user, CancellationToken cancellationToken)
        {
            user.AccessFailedCount++;
            _users[user.Id] = user;
            return Task.FromResult(user.AccessFailedCount);
        }

        /// <summary>
        /// Resets the access failed count for the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A completed task.</returns>
        public Task ResetAccessFailedCountAsync(TestAppIdentityUser user, CancellationToken cancellationToken)
        {
            user.AccessFailedCount = 0;
            _users[user.Id] = user;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Gets a value indicating whether lockout is enabled for the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True if lockout is enabled; otherwise, false.</returns>
        public Task<bool> GetLockoutEnabledAsync(TestAppIdentityUser user, CancellationToken cancellationToken)
            => Task.FromResult(user.LockoutEnabled);

        /// <summary>
        /// Sets a value indicating whether lockout is enabled for the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="enabled">True to enable lockout; otherwise, false.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A completed task.</returns>
        public Task SetLockoutEnabledAsync(TestAppIdentityUser user, bool enabled, CancellationToken cancellationToken)
        {
            user.LockoutEnabled = enabled;
            _users[user.Id] = user;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Gets the lockout end date for the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The lockout end date, if set; otherwise, null.</returns>
        public Task<DateTimeOffset?> GetLockoutEndDateAsync(TestAppIdentityUser user, CancellationToken cancellationToken)
            => Task.FromResult(user.LockoutEnd);

        /// <summary>
        /// Sets the lockout end date for the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="lockoutEnd">The lockout end date to set.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A completed task.</returns>
        public Task SetLockoutEndDateAsync(TestAppIdentityUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
        {
            user.LockoutEnd = lockoutEnd;
            _users[user.Id] = user;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Disposes the store. No resources to release.
        /// </summary>
        public void Dispose() { }
    }
}