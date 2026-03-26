namespace StruttonTechnologies.Core.Identity.Data
{
    /// <summary>
    /// Default claims seeded into production.
    /// </summary>
    public static class KnownClaims
    {
        public const string Email = "email";
        public const string Role = "role";

        public static readonly string[] All = { Email, Role };

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
