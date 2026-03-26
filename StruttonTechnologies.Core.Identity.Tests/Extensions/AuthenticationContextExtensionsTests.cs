using StruttonTechnologies.Core.Identity.Extensions;
using StruttonTechnologies.Core.Identity.Models;
using StruttonTechnologies.Core.Identity.Validators.Composite;
using StruttonTechnologies.Core.Identity.Validators.Identity;

namespace StruttonTechnologies.Core.Identity.Tests.Extensions
{
    /// <summary>
    /// Contains test scenarios for <see cref="AuthenticationContextExtensions"/>.
    /// </summary>
    public class AuthenticationContextExtensionsTests
    {
        [Fact]
        public void ToAuthContext_WithValidContext_MapsValues()
        {
            AuthenticationContext context = new()
            {
                ProviderName = "Local",
                SessionId = Guid.NewGuid().ToString(),
                TenantId = Guid.NewGuid().ToString(),
                Status = IdentityStatus.Active,
            };

            AuthContext result = context.ToAuthContext();

            Assert.Equal(context.ProviderName, result.Provider);
            Assert.Equal(context.SessionId, result.SessionId);
            Assert.Equal(context.Status, result.Status);
        }

        [Fact]
        public void ToAuthContext_WithNullContext_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => AuthenticationContextExtensions.ToAuthContext(null!));
        }
    }
}
