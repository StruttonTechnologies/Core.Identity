using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Test.Data
{
    /// <summary>
    /// Supported identity providers for test scenarios.
    /// Order is intentional and mirrors production.
    /// </summary>
    public static class KnownIdentityProviders
    {
        public const string Google = "Google";
        public const string Microsoft = "Microsoft";
        public const string GitHub = "GitHub";
        public const string Okta = "Okta";
        public const string Auth0 = "Auth0";

        public static readonly string[] All =
        {
            Google,
            Microsoft,
            GitHub,
            Okta,
            Auth0
        };
        public static string[] First(int count = 1) =>
            All.Take(Math.Max(count, 1)).ToArray();

        public static string[] Last(int count = 1) =>
            All.Skip(Math.Max(All.Length - count, 0)).ToArray();
    }
}
