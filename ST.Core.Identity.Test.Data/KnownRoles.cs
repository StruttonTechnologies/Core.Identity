using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Test.Data
{
    /// <summary>
    /// Provides a centralized set of role names for test scenarios.
    /// Order is intentional: Admin > Member > Guest.
    /// </summary>
    public static class KnownRoles
    {
        public const string Admin = "Admin";
        public const string Member = "Member";
        public const string Guest = "Guest";

        public static readonly string[] All = { Admin, Member, Guest };
        public static string[] First(int count = 1) =>
            All.Take(Math.Max(count, 1)).ToArray();

        public static string[] Last(int count = 1) =>
            All.Skip(Math.Max(All.Length - count, 0)).ToArray();
    }
}
