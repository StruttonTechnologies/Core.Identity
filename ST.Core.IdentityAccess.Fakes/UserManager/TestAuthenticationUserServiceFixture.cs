using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ST.Core.Identity.Fakes.Models;
using ST.Core.Identity.Fakes.Factories;
using ST.Core.IdentityAccess.Fakes.Stores;
using ST.Core.IdentityAccess.UserManager.Authentication;
using ST.Core.Identity.Models;

namespace ST.Core.IdentityAccess.Fakes.UserManager
{

    /// <summary>
    /// Test fixture for <see cref="AuthenticationUserService{TUser, TKey}"/>.
    /// Provides a test-safe <see cref="UserManager{TUser}"/> and logger.
    /// </summary>
    public class TestAuthenticationUserServiceFixture
    {
        /// <summary>
        /// The identity key type used for testing.
        /// </summary>
        private readonly IdentityKey _identityKey = IdentityKey.Guid;

        /// <summary>
        /// Gets the test user manager instance.
        /// </summary>
        public TestUserManager UserManager { get; }

        /// <summary>
        /// Gets the logger instance for <see cref="AuthenticationUserService{TUser, TKey}"/>.
        /// </summary>
        public ILogger<AuthenticationUserService<TestAppIdentityUser, Guid>> Logger { get; }

        /// <summary>
        /// Gets the testable authentication user service instance.
        /// </summary>
        public AuthenticationUserService<TestAppIdentityUser, Guid> Service { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestAuthenticationUserServiceFixture"/> class.
        /// </summary>
        public TestAuthenticationUserServiceFixture()
        {
            var store = new InMemoryUserStore();
            UserManager = new TestUserManager(store);
            Logger = new LoggerFactory().CreateLogger<AuthenticationUserService<TestAppIdentityUser, Guid>>();
            Service = new TestableAuthenticationUserService(UserManager, Logger);
        }

        /// <summary>
        /// Creates a test user with the specified username and optional audit information.
        /// </summary>
        /// <param name="username">The username for the test user. Defaults to "testuser".</param>
        /// <param name="withAudit">Whether to include audit information in the test user.</param>
        /// <returns>A new <see cref="TestAppIdentityUser"/> instance.</returns>
        public TestAppIdentityUser CreateTestUser(string username = "testuser", bool withAudit = false)
        {
            var person = withAudit
                ? TestAppPersonFactory.CreateFullAudit(Guid.NewGuid(), Guid.NewGuid())
                : TestAppPersonFactory.CreateDefault();

            return new TestAppIdentityUser
            {
                UserName = username,
                Person = person
            };
        }

        /// <summary>
        /// Testable implementation of <see cref="AuthenticationUserService{TUser, TKey}"/> for <see cref="TestAppIdentityUser"/>.
        /// </summary>
        private class TestableAuthenticationUserService : AuthenticationUserService<TestAppIdentityUser, Guid>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TestableAuthenticationUserService"/> class.
            /// </summary>
            /// <param name="userManager">The user manager to use for user operations.</param>
            /// <param name="logger">The logger instance.</param>
            public TestableAuthenticationUserService(
                UserManager<TestAppIdentityUser> userManager,
                ILogger<AuthenticationUserService<TestAppIdentityUser, Guid>> logger)
                : base(userManager, logger) { }
        }
    }
}