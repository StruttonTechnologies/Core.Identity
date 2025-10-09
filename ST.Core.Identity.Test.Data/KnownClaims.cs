using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Test.Data
{
    /// <summary>
    /// Provides reusable claim types for test scenarios.
    /// </summary>
    public static class KnownClaims
    {
        public const string Email = "email";
        public const string Role = "role";

        public static readonly string[] All = { Email, Role };
        public static string[] First(int count = 1) =>
            All.Take(Math.Max(count, 1)).ToArray();

        public static string[] Last(int count = 1) =>
            All.Skip(Math.Max(All.Length - count, 0)).ToArray();
    }
}
