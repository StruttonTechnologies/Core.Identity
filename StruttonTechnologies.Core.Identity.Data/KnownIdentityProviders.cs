namespace StruttonTechnologies.Core.Identity.Data
{
    /// <summary>
    /// Supported identity providers for authentication.
    /// Order is intentional and should not be changed without review.
    /// </summary>
    public static class KnownIdentityProviders
    {
        public const string Google = "Google";
        public const string Microsoft = "Microsoft";
        public const string GitHub = "GitHub";
        public const string Okta = "Okta";
        public const string Auth0 = "Auth0";
        public const string Local = "Local";

        public static readonly string[] All =
        {
            Google,
            Microsoft,
            GitHub,
            Okta,
            Auth0,
            Local
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
