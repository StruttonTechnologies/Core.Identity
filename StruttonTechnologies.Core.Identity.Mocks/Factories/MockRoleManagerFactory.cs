using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore.Identity;

using Moq;

using StruttonTechnologies.Core.Identity.Test.Data;

namespace StruttonTechnologies.Core.Identity.Mocks.Factories
{
    /// <summary>
    /// Provides a factory for creating mocked RoleManager instances
    /// preconfigured with KnownRoles from Test.Data.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class MockRoleManagerFactory
    {
        public static Mock<RoleManager<IdentityRole>> Create()
        {
            Mock<IRoleStore<IdentityRole>> store = new Mock<IRoleStore<IdentityRole>>();
            Mock<RoleManager<IdentityRole>> mgr = new Mock<RoleManager<IdentityRole>>(
                store.Object, null!, null!, null!, null!);

            foreach (string role in KnownRoles.All)
            {
                mgr.Setup(m => m.RoleExistsAsync(role)).ReturnsAsync(true);
            }

            return mgr;
        }
    }
}
