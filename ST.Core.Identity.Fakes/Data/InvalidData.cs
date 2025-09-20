using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Fakes.Data
{
    /// <summary>
    /// Provides invalid test data for identity-related scenarios.
    /// </summary>
    public static class InvalidData
    {
        /// <summary>
        /// Gets a collection of invalid session ID values.
        /// </summary>
        public static IEnumerable<object[]> SessionIds =>
            [
                [null!],
                [""],
                ["not-a-guid"],
                [Guid.Empty.ToString()]
            ];

        /// <summary>
        /// Gets a collection of invalid tenant ID values.
        /// </summary>
        public static IEnumerable<object[]> TenantIds =>
            [
                [null!],
                [""],
                ["not-a-guid"],
                [Guid.Empty.ToString()]
            ];

        /// <summary>
        /// Gets a collection of invalid scope values.
        /// </summary>
        public static IEnumerable<object[]> Scopes =>
            [
                [new[] { "unknown:scope" }],
                [new[] { "read:user", "invalid:scope" }],
                [null!]
            ];

        /// <summary>
        /// Gets a collection of invalid role values.
        /// </summary>
        public static IEnumerable<object[]> Roles =>
            [
                [null!],
                [""],
                ["UnknownRole"]
            ];

        /// <summary>
        /// Gets a collection of invalid provider values.
        /// </summary>
        public static IEnumerable<object[]> Providers =>
            [
                [null!],
                [""],
                ["UnknownSSO"]
            ];

        /// <summary>
        /// Gets a collection of invalid username values.
        /// </summary>
        public static IEnumerable<object[]> Usernames =>
            [
                [null!],
                [""],
                ["admin"],
                ["root"],
                ["user name"],
                ["!"]
            ];
    }
}
