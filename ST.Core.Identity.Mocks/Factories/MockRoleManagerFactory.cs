using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Test.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Mocks.Factories
{
    /// <summary>
    /// Provides a factory for creating mocked RoleManager instances
    /// preconfigured with KnownRoles from Test.Data.
    /// </summary>
    public static class MockRoleManagerFactory
    {
        public static Mock<RoleManager<IdentityRole>> Create()
        {
            var store = new Mock<IRoleStore<IdentityRole>>();
            var mgr = new Mock<RoleManager<IdentityRole>>(
                store.Object, null!, null!, null!, null!);

            foreach (var role in KnownRoles.All)
            {
                mgr.Setup(m => m.RoleExistsAsync(role)).ReturnsAsync(true);
            }

            return mgr;
        }
    }
}
