using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Data
{
    /// <summary>
    /// Default users seeded into production.
    /// </summary>
    public static class KnownUsers
    {
        public const string AdminUserName = "admin@app.local";
        public const string DefaultUserName = "user@app.local";

        public static readonly string[] All = { AdminUserName, DefaultUserName };
    }
}
