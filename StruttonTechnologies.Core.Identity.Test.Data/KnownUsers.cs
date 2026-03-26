using StruttonTechnologies.Core.Identity.Domain.Entities;

namespace StruttonTechnologies.Core.Identity.Test.Data
{
    /// <summary>
    /// Provides known users for test scenarios.
    /// </summary>
    public static class KnownUsers
    {
        /// <summary>
        /// The default administrator user name.
        /// </summary>
        public const string AdminUserName = "admin@app.local";

        /// <summary>
        /// The default standard user name.
        /// </summary>
        public const string DefaultUserName = "user@app.local";

        /// <summary>
        /// Gets all known user names.
        /// </summary>
        public static readonly string[] All = { AdminUserName, DefaultUserName };

        /// <summary>
        /// Gets the default test user for authentication scenarios.
        /// </summary>
        public static readonly IdentityUser<Guid> Default = new IdentityUser<Guid>
        {
            UserName = DefaultUserName,
            Email = DefaultUserName,
            Id = Guid.Parse("b7e6a1e2-2c4a-4e7a-9c2a-1a2b3c4d5e6f"),
        };

        /// <summary>
        /// Gets the administrator test user for elevated access scenarios.
        /// </summary>
        public static readonly IdentityUser<Guid> Admin = new IdentityUser<Guid>
        {
            UserName = AdminUserName,
            Email = AdminUserName,
            Id = Guid.Parse("a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d"),
        };

        /// <summary>
        /// Returns the first known user names.
        /// </summary>
        /// <param name="count">The number of user names to return.</param>
        /// <returns>An array containing the first known user names.</returns>
        public static string[] First(int count = 1)
        {
            return All.Take(Math.Max(count, 1)).ToArray();
        }

        /// <summary>
        /// Returns the last known user names.
        /// </summary>
        /// <param name="count">The number of user names to return.</param>
        /// <returns>An array containing the last known user names.</returns>
        public static string[] Last(int count = 1)
        {
            return All.Skip(Math.Max(All.Length - count, 0)).ToArray();
        }

        /// <summary>
        /// Creates a test user with the specified role naming pattern.
        /// </summary>
        /// <param name="role">The role to embed in the generated user identity.</param>
        /// <returns>A new test user instance.</returns>
        public static IdentityUser<Guid> WithRole(string role)
        {
            return new IdentityUser<Guid>
            {
                Id = Guid.NewGuid(),
                UserName = $"user+{role}@app.local",
                Email = $"user+{role}@app.local",
            };
        }
    }
}
