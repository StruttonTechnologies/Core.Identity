using System.Diagnostics.CodeAnalysis;

namespace StruttonTechnologies.Core.Identity.Data
{
    /// <summary>
    /// Default users seeded into production.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class KnownUsers
    {
        public const string AdminUserName = "admin@app.local";
        public const string DefaultUserName = "user@app.local";

        public static readonly string[] All = { AdminUserName, DefaultUserName };
    }
}
