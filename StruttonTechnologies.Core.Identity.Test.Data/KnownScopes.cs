namespace StruttonTechnologies.Core.Identity.Test.Data
{
    public static class KnownScopes
    {
        public const string OpenId = "openid";
        public const string Profile = "profile";
        public const string ApiRead = "api.read";
        public const string ApiWrite = "api.write";

        public static readonly string[] All =
        {
            OpenId,
            Profile,
            ApiRead,
            ApiWrite
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
