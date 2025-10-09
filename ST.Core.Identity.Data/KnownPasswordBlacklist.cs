using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Data
{
    /// <summary>
    /// Centralized list of disallowed passwords that are too common or insecure.
    /// </summary>
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
        public static string[] First(int count = 1) =>
        All.Take(Math.Max(count, 1)).ToArray();

        public static string[] Last(int count = 1) =>
            All.Skip(Math.Max(All.Length - count, 0)).ToArray();
    }
}
