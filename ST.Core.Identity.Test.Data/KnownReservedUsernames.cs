using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Test.Data
{
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
        public static string[] First(int count = 1) =>
            All.Take(Math.Max(count, 1)).ToArray();

        public static string[] Last(int count = 1) =>
            All.Skip(Math.Max(All.Length - count, 0)).ToArray();
    }
}
