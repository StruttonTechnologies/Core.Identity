using ST.Core.Identity.Validators.Identity;

namespace ST.Core.Identity.Tests.Validators
{
    public class IdentityProviderValidatorTests
    {
        private readonly IdentityProviderValidator _validator = new();

        [Theory]
        [InlineData("Google")]
        [InlineData("github")]
        [InlineData("OKTA")]
        public void Should_Return_Success_For_Known_Providers(string provider)
        {
            var result = _validator.Validate(provider);
            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData("UnknownSSO")]
        public void Should_Return_Failure_For_Unknown_Providers(string provider)
        {
            var result = _validator.Validate(provider);
            Assert.False(result.IsValid);
        }
    }
}