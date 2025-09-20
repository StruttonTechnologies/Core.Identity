using ST.Core.Identity.Data;
using ST.Core.Identity.Dtos.Authentication;
using ST.Core.Identity.Fakes.Data;
using ST.Core.Identity.Models;
using ST.Core.Identity.Validators.Composite;

namespace ST.Core.Identity.Infrastructure.EF.Tests.Validators
{
    public class AuthenticationContextValidatorTests
    {
        private readonly AuthenticationContextValidator _validator = new();

        public static IEnumerable<object[]> ValidContexts =>
            new[]
            {
                new object[]
                {
                    new AuthenticationContext
                    {
                        SessionId = Guid.NewGuid().ToString(),
                        TenantId = Guid.NewGuid().ToString(),
                        Scopes = IdentitySeed.AllowedScopes.Take(2).ToArray()
                    }
                },
                new object[]
                {
                    new AuthenticationContext
                    {
                        SessionId = Guid.NewGuid().ToString(),
                        TenantId = Guid.NewGuid().ToString(),
                        Scopes = new[] { IdentitySeed.AllowedScopes.First() }
                    }
                }
            };

        [Theory]
        [MemberData(nameof(ValidContexts))]
        public void Should_Return_Success_For_Valid_Contexts(AuthenticationContext context)
        {
            var result = _validator.Validate(context);
            Assert.True(result.IsSuccess);
        }

        [Theory]
        [MemberData(nameof(InvalidData.SessionIds), MemberType = typeof(InvalidData))]
        public void Should_Fail_When_SessionId_Is_Invalid(string sessionId)
        {
            var context = new AuthenticationContext
            {
                SessionId = sessionId,
                TenantId = Guid.NewGuid().ToString(),
                Scopes = IdentitySeed.AllowedScopes.Take(1)
            };

            var result = _validator.Validate(context);
            Assert.False(result.IsSuccess);
            Assert.Equal("InvalidSessionId", result.Code);
        }

        [Theory]
        [MemberData(nameof(InvalidData.TenantIds), MemberType = typeof(InvalidData))]
        public void Should_Fail_When_TenantId_Is_Invalid(string tenantId)
        {
            var context = new AuthenticationContext
            {
                SessionId = Guid.NewGuid().ToString(),
                TenantId = tenantId,
                Scopes = IdentitySeed.AllowedScopes.Take(1)
            };

            var result = _validator.Validate(context);
            Assert.False(result.IsSuccess);
            Assert.Equal("InvalidTenantId", result.Code);
        }

        [Theory]
        [MemberData(nameof(InvalidData.Scopes), MemberType = typeof(InvalidData))]
        public void Should_Fail_When_Scopes_Are_Invalid(IEnumerable<string> scopes)
        {
            var context = new AuthenticationContext
            {
                SessionId = Guid.NewGuid().ToString(),
                TenantId = Guid.NewGuid().ToString(),
                Scopes = scopes
            };

            var result = _validator.Validate(context);
            Assert.False(result.IsSuccess);
            Assert.Equal("InvalidScope", result.Code);
        }

        [Fact]
        public void Should_Fail_When_Multiple_Fields_Are_Invalid()
        {
            var context = new AuthenticationContext
            {
                SessionId = "",
                TenantId = "not-a-guid",
                Scopes = new[] { "invalid:scope" }
            };

            var result = _validator.Validate(context);
            Assert.False(result.IsSuccess);
            // Depending on implementation, this may return first failure or aggregate
        }
    }
}