using Microsoft.AspNetCore.Identity;
using ST.Core.Identity.Fakes.Models;

namespace ST.Core.IdentityAccess.Fakes.UserManager.Providers
{
    /// <summary>
    /// A stub token provider for password reset.
    /// Always returns a fixed token and validates only against that token.
    /// </summary>
    public class StubPasswordResetTokenProvider : IUserTwoFactorTokenProvider<TestAppIdentityUser>
    {
        /// <summary>
        /// The fixed token value to return and validate against.
        /// </summary>
        public string FixedToken { get; set; } = "stub-token";

        /// <summary>
        /// Returns the fixed token value.
        /// </summary>
        public Task<string> GenerateAsync(string purpose, UserManager<TestAppIdentityUser> manager, TestAppIdentityUser user)
            => Task.FromResult(FixedToken);

        /// <summary>
        /// Validates the token by comparing it to the fixed value.
        /// </summary>
        public Task<bool> ValidateAsync(string purpose, string token, UserManager<TestAppIdentityUser> manager, TestAppIdentityUser user)
            => Task.FromResult(token == FixedToken);

        /// <summary>
        /// Indicates that token generation is supported.
        /// </summary>
        public Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<TestAppIdentityUser> manager, TestAppIdentityUser user)
            => Task.FromResult(true);
    }
}