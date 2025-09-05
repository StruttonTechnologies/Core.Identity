using Microsoft.AspNetCore.Identity;
using ST.Core.Identity.Testing.Toolkit.Builders;
using ST.Core.Identity.Testing.Toolkit.Models;
using System.Diagnostics.CodeAnalysis;

namespace ST.Core.Identity.Testing.Toolkit.Extensions
{

    /// <summary>
    /// Provides methods for seeding default test users.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class TestUserSeeder
    {
        /// <summary>
        /// Gets a collection of default <see cref="TestUser"/> instances for testing.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerable{TestUser}"/> containing the default test users.
        /// </returns>
        public static IEnumerable<TestUser> GetDefaultUsers()
        {
            return new[]
            {
                    new TestUserBuilder().WithUserName("alpha").WithRoles("Admin").Build(),
                    new TestUserBuilder().WithUserName("beta").WithRoles("User").Build()
                };
        }
    }
}

