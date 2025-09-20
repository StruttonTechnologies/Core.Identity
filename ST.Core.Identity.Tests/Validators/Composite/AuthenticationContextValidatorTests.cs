using ST.Core.Identity.Data;
using ST.Core.Identity.Extensions;
using ST.Core.Identity.Fakes.Data;
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

        [Theory]
        [MemberData(nameof(InvalidData.SessionIds), MemberType = typeof(InvalidData))]
        public void Should_Fail_When_SessionId_Is_Invalid(string sessionId)
        {
            var context = new AuthenticationContext
            {
                ProviderName = "Local",
                SessionId = sessionId,
                TenantId = Guid.NewGuid().ToString(),
                Scopes = IdentitySeed.AllowedScopes.Take(1),
                Status = IdentityStatus.Active
            };

            var result = _validator.Validate(context.ToAuthContext());
            Assert.False(result.IsSuccess);
            Assert.Equal("InvalidSessionId", result.Code);
        }

        [Theory]
        [MemberData(nameof(InvalidData.TenantIds), MemberType = typeof(InvalidData))]
        public void Should_Fail_When_TenantId_Is_Invalid(string tenantId)
        {
            var context = new AuthenticationContext
            {
                ProviderName = "Local",
                SessionId = Guid.NewGuid().ToString(),
                TenantId = tenantId,
                Scopes = IdentitySeed.AllowedScopes.Take(1),
                Status = IdentityStatus.Active
            };

            var result = _validator.Validate(context.ToAuthContext());
            Assert.False(result.IsSuccess);
            Assert.Equal("InvalidTenantId", result.Code);
        }

        [Theory]
        [MemberData(nameof(InvalidData.Scopes), MemberType = typeof(InvalidData))]
        public void Should_Fail_When_Scopes_Are_Invalid(IEnumerable<string> scopes)
        {
            var context = new AuthenticationContext
            {
                ProviderName = "Local",
                SessionId = Guid.NewGuid().ToString(),
                TenantId = Guid.NewGuid().ToString(),
                Scopes = scopes,
                Status = IdentityStatus.Active
            };

            var result = _validator.Validate(context.ToAuthContext());
            Assert.False(result.IsSuccess);
            Assert.Equal("InvalidScope", result.Code);
        }

        [Fact]
        public void Should_Fail_When_Multiple_Fields_Are_Invalid()
        {
            var context = new AuthenticationContext
            {
                ProviderName = "UnknownSSO",
                SessionId = "",
                TenantId = "not-a-guid",
                Scopes = new[] { "invalid:scope" },
                Status = IdentityStatus.Suspended
            };

            var result = _validator.Validate(context.ToAuthContext());
            Assert.False(result.IsSuccess);
            // Depending on implementation, this may return first failure or aggregate
        }
    }
}