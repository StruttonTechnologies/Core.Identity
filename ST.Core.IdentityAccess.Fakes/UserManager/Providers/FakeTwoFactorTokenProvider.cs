using Microsoft.AspNetCore.Identity;
using ST.Core.Identity.Fakes.Models;
using ST.Core.Identity.Stub.Entities;

namespace ST.Core.IdentityAccess.Fakes.UserManager.Providers
{
    /// <summary>
    /// Fake token provider for two-factor authentication.
    /// Supports toggleable failure simulation via <see cref="ShouldThrow"/>.
    /// </summary>
    public class FakeTwoFactorTokenProvider : IUserTwoFactorTokenProvider<StubUser>
    {
        /// <summary>
        /// Indicates whether token generation should simulate failure.
        /// </summary>
        public bool ShouldThrow { get; set; }

        /// <summary>
        /// Generates a fixed token or throws an exception if <see cref="ShouldThrow"/> is true.
        /// </summary>
        public Task<string> GenerateAsync(string purpose, UserManager<StubUser> manager, StubUser user)
        {
            if (ShouldThrow)
                throw new InvalidOperationException("Simulated failure");

            return Task.FromResult("valid-token");
        }

        /// <summary>
        /// Validates the token by comparing it to the fixed value.
        /// </summary>
        public Task<bool> ValidateAsync(string purpose, string token, UserManager<StubUser> manager, StubUser user)
            => Task.FromResult(token == "valid-token");

        /// <summary>
        /// Indicates that token generation is always supported.
        /// </summary>
        public Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<StubUser> manager, StubUser user)
            => Task.FromResult(true);
    }
}