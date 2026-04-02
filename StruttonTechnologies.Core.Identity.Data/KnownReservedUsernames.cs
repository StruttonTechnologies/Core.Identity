using System.Diagnostics.CodeAnalysis;

namespace StruttonTechnologies.Core.Identity.Data
{
    [ExcludeFromCodeCoverage]
    public static class KnownReservedUsernames
    {
        // Intentionally case-insensitive matching will be handled by the validator
        public static readonly string[] All =
        {
            "admin",
            "administrator",
            "system",
            "root",
            "service",
            "support",
            "moderator"
        };

        public static string[] First(int count = 1)
        {
            return All.Take(Math.Max(count, 1)).ToArray();
        }

        public static string[] Last(int count = 1)
        {
            return All.Skip(Math.Max(All.Length - count, 0)).ToArray();
        }
    }
}
