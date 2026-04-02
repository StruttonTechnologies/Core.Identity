using System.Diagnostics.CodeAnalysis;

namespace StruttonTechnologies.Core.Identity.Data
{
    /// <summary>
    /// Centralized list of disallowed passwords that are too common or insecure.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class KnownPasswordBlacklist
    {
        public static readonly string[] All =
        {
            "password",
            "123456",
            "qwerty",
            "letmein",
            "admin",
            "welcome"

                // Add more as needed
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
