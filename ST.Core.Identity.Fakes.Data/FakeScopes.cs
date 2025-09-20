using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Fakes.Data
{
    namespace ST.Core.Identity.Fakes.Data
    {
        public static class FakeScopes
        {
            public static IEnumerable<object[]> Valid =>
                new List<object[]>
                {
                new object[] { new[] { "read:user", "write:profile" } },
                new object[] { new[] { "admin:tenant" } },
                new object[] { new[] { "manage:roles", "read:settings" } }
                };

            public static IEnumerable<object[]> Invalid =>
                new List<object[]>
                {
                new object[] { null },
                new object[] { new[] { "unknown:scope" } },
                new object[] { new[] { "read:user", "invalid:scope" } }
                };
        }
    }
}
