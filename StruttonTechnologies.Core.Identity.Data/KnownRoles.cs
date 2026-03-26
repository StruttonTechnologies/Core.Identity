namespace StruttonTechnologies.Core.Identity.Data
{
    /// <summary>
    /// Supported roles for production seeding.
    /// Order is intentional and should not be changed without review.
    /// </summary>
    public static class KnownRoles
    {
        public const string Admin = "Admin";
        public const string Member = "Member";
        public const string Guest = "Guest";
        public static readonly string[] All = { Admin, Member, Guest };

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
