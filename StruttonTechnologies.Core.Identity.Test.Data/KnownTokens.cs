namespace StruttonTechnologies.Core.Identity.Test.Data
{
    /// <summary>
    /// Provides static JWTs or token placeholders for test scenarios.
    /// </summary>
    public static class KnownTokens
    {
        public const string ValidToken = "valid-token-placeholder";
        public const string ExpiredToken = "expired-token-placeholder";
        public const string InvalidToken = "invalid-token-placeholder";

        public static readonly string[] All = { ValidToken, ExpiredToken, InvalidToken };
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
