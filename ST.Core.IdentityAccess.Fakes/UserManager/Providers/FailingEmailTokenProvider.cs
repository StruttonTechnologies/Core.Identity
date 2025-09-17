using Microsoft.AspNetCore.Identity;
using ST.Core.Identity.Fakes.Models;

namespace ST.Core.IdentityAccess.Fakes.UserManager.Providers
{
    /// <summary>
    /// A token provider that always throws when generating or validating email tokens.
    /// Used to simulate failure scenarios in email confirmation workflows.
    /// </summary>
    public class FailingEmailTokenProvider : IUserTwoFactorTokenProvider<TestAppIdentityUser>
    {
        /// <summary>
        /// Always throws an exception when attempting to generate a token.
        /// </summary>
        public Task<string> GenerateAsync(string purpose, UserManager<TestAppIdentityUser> manager, TestAppIdentityUser user)
            => throw new InvalidOperationException("Simulated email token generation failure");

        /// <summary>
        /// Always throws an exception when attempting to validate a token.
        /// </summary>
        public Task<bool> ValidateAsync(string purpose, string token, UserManager<TestAppIdentityUser> manager, TestAppIdentityUser user)
            => throw new InvalidOperationException("Simulated email token validation failure");

        /// <summary>
        /// Indicates that token generation is not supported.
        /// </summary>
        public Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<TestAppIdentityUser> manager, TestAppIdentityUser user)
            => Task.FromResult(false);
    }
}