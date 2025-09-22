using ST.Core.Identity.Data;
using ST.Core.Identity.Extensions;
using ST.Core.Identity.Models;
using ST.Core.Identity.Validators.Access;
using ST.Core.Identity.Validators.Composite;
using ST.Core.Identity.Validators.Identity;

namespace ST.Core.Identity.Tests.Validators.Composite
{
    public class AuthenticationContextValidatorTests
    {
        private readonly AuthenticationContextValidator _validator;

        public AuthenticationContextValidatorTests()
        {
            _validator = new AuthenticationContextValidator(
                new IdentityProviderValidator(),
                new SessionIdValidator(),
                new IdentityStatusValidator()
            );
        }

        public static IEnumerable<object[]> ValidContexts =>
            new[]
            {
                new object[]
                {
                    new AuthenticationContext
                    {
                        ProviderName = "Local",
                        SessionId = Guid.NewGuid().ToString(),
                        TenantId = Guid.NewGuid().ToString(),
                        Scopes = IdentitySeed.AllowedScopes.Take(2).ToArray(),
                        Status = IdentityStatus.Active
                    }
                },
                new object[]
                {
                    new AuthenticationContext
                    {
                        ProviderName = "Google",
                        SessionId = Guid.NewGuid().ToString(),
                        TenantId = Guid.NewGuid().ToString(),
                        Scopes = new[] { IdentitySeed.AllowedScopes.First() },
                        Status = IdentityStatus.Active
                    }
                }
            };

        [Theory]
        [MemberData(nameof(ValidContexts))]
        public void Should_Return_Success_For_Valid_Contexts(AuthenticationContext context)
        {
            var result = _validator.Validate(context.ToAuthContext());
            Assert.True(result.IsSuccess);
        }
    }
}