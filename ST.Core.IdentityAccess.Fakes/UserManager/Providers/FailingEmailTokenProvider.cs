using Microsoft.AspNetCore.Identity;
using ST.Core.Identity.Stub.Entities;

namespace ST.Core.IdentityAccess.Fakes.UserManager.Providers
{
    /// <summary>
    /// A token provider that always throws when generating or validating email tokens.
    /// Used to simulate failure scenarios in email confirmation workflows.
    /// </summary>
    public class FailingEmailTokenProvider : IUserTwoFactorTokenProvider<StubUser>
    {
        /// <summary>
        /// Always throws an exception when attempting to generate a token.
        /// </summary>
        public Task<string> GenerateAsync(string purpose, UserManager<StubUser> manager, StubUser user)
            => throw new InvalidOperationException("Simulated email token generation failure");

        /// <summary>
        /// Always throws an exception when attempting to validate a token.
        /// </summary>
        public Task<bool> ValidateAsync(string purpose, string token, UserManager<StubUser> manager, StubUser user)
            => throw new InvalidOperationException("Simulated email token validation failure");

        /// <summary>
        /// Indicates that token generation is not supported.
        /// </summary>
        public Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<StubUser> manager, StubUser user)
            => Task.FromResult(false);
    }
}