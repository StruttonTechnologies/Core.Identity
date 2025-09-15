using Microsoft.AspNetCore.Identity;
using ST.Core.Identity.Testing.Setup.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ST.Core.Identity.Infrastructure.Tests.SetUp.Models
{
    /// <summary>
    /// A minimal IUserPasswordStore implementation for testing UserManager behavior.
    /// Simulates persistence and lookup logic without hitting a real database.
    /// </summary>
    internal class MockUserStore : IUserPasswordStore<TestAppIdentityUser>
    {
        public Task<IdentityResult> CreateAsync(TestAppIdentityUser user, CancellationToken cancellationToken)
            => Task.FromResult(IdentityResult.Success);

        public Task<IdentityResult> UpdateAsync(TestAppIdentityUser user, CancellationToken cancellationToken)
            => Task.FromResult(IdentityResult.Success);

        public Task<IdentityResult> DeleteAsync(TestAppIdentityUser user, CancellationToken cancellationToken)
            => Task.FromResult(IdentityResult.Success);

        public Task<TestAppIdentityUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
            => Task.FromResult<TestAppIdentityUser?>(null);

        public Task<TestAppIdentityUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
            => Task.FromResult<TestAppIdentityUser?>(null);

        public Task<string?> GetUserIdAsync(TestAppIdentityUser user, CancellationToken cancellationToken)
            => Task.FromResult(user.Id.ToString());

        public Task<string?> GetUserNameAsync(TestAppIdentityUser user, CancellationToken cancellationToken)
            => Task.FromResult(user.UserName);

        public Task SetUserNameAsync(TestAppIdentityUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public Task<string?> GetNormalizedUserNameAsync(TestAppIdentityUser user, CancellationToken cancellationToken)
            => Task.FromResult(user.NormalizedUserName);

        public Task SetNormalizedUserNameAsync(TestAppIdentityUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetPasswordHashAsync(TestAppIdentityUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task<string?> GetPasswordHashAsync(TestAppIdentityUser user, CancellationToken cancellationToken)
            => Task.FromResult(user.PasswordHash);

        public Task<bool> HasPasswordAsync(TestAppIdentityUser user, CancellationToken cancellationToken)
            => Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));

        public void Dispose() { }
    }
}
