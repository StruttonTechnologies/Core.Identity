using Microsoft.AspNetCore.Identity;
using ST.Core.Identity.Fakes.Models;

namespace ST.Core.IdentityAccess.Fakes.UserManager.Providers
{
    /// <summary>
    /// Fake token provider for two-factor authentication.
    /// Supports toggleable failure simulation via <see cref="ShouldThrow"/>.
    /// </summary>
    public class FakeTwoFactorTokenProvider : IUserTwoFactorTokenProvider<TestAppIdentityUser>
    {
        /// <summary>
        /// Indicates whether token generation should simulate failure.
        /// </summary>
        public bool ShouldThrow { get; set; }

        /// <summary>
        /// Generates a fixed token or throws an exception if <see cref="ShouldThrow"/> is true.
        /// </summary>
        public Task<string> GenerateAsync(string purpose, UserManager<TestAppIdentityUser> manager, TestAppIdentityUser user)
        {
            if (ShouldThrow)
                throw new InvalidOperationException("Simulated failure");

            return Task.FromResult("valid-token");
        }

        /// <summary>
        /// Validates the token by comparing it to the fixed value.
        /// </summary>
        public Task<bool> ValidateAsync(string purpose, string token, UserManager<TestAppIdentityUser> manager, TestAppIdentityUser user)
            => Task.FromResult(token == "valid-token");

        /// <summary>
        /// Indicates that token generation is always supported.
        /// </summary>
        public Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<TestAppIdentityUser> manager, TestAppIdentityUser user)
            => Task.FromResult(true);
    }
}