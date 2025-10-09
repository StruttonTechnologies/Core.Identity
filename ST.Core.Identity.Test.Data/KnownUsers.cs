using Microsoft.AspNetCore.Identity;

namespace ST.Core.Identity.Test.Data
{
    /// <summary>
    /// Default users seeded into production.
    /// </summary>
    public static class KnownUsers
    {
        public const string AdminUserName = "admin@app.local";
        public const string DefaultUserName = "user@app.local";

        public static readonly string[] All = { AdminUserName, DefaultUserName };

        /// <summary>
        /// Returns the first N known usernames. Defaults to 1.
        /// </summary>
        public static string[] First(int count = 1) =>
            All.Take(Math.Max(count, 1)).ToArray();

        /// <summary>
        /// Returns the last N known usernames. Defaults to 1.
        /// </summary>
        public static string[] Last(int count = 1) =>
            All.Skip(Math.Max(All.Length - count, 0)).ToArray();

        /// <summary>
        /// Default test user for authentication scenarios.
        /// </summary>
        public static readonly IdentityUser Default = new IdentityUser
        {
            UserName = DefaultUserName,
            Email = DefaultUserName,
            Id = "default-user-id"
        };

        /// <summary>
        /// Admin test user for elevated access scenarios.
        /// </summary>
        public static readonly IdentityUser Admin = new IdentityUser
        {
            UserName = AdminUserName,
            Email = AdminUserName,
            Id = "admin-user-id"
        };

        public static IdentityUser WithRole(string role) => new IdentityUser
        {
            Id = Guid.NewGuid().ToString(),
            UserName = $"user+{role}@app.local",
            Email = $"user+{role}@app.local"
        };


    }
}