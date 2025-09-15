using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Infrastructure.Configuration
{
    /// <summary>
    /// Specifies the supported database providers for the identity infrastructure.
    /// </summary>
    public enum IdentityDatabaseProvider
    {
        /// <summary>
        /// Microsoft SQL Server database provider.
        /// </summary>
        SqlServer,

        /// <summary>
        /// PostgreSQL database provider.
        /// </summary>
        PostgreSql,

        /// <summary>
        /// MySQL database provider.
        /// </summary>
        MySql,

        /// <summary>
        /// SQLite database provider.
        /// </summary>
        Sqlite
    }
}
