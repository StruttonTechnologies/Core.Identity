using StruttonTechnologies.Core.ToolKit.Validation.Models;

namespace StruttonTechnologies.Core.Identity.Tests.Validators.Identity
{
    /// <summary>
    /// Contains test scenarios for <see cref="StruttonTechnologies.Core.Identity.Validators.Identity.IdentityStatusValidator"/>.
    /// </summary>
    public class IdentityStatusValidatorTests
    {
        private readonly StruttonTechnologies.Core.Identity.Validators.Identity.IdentityStatusValidator _sut = new();

        [Fact]
        public void Validate_WithActiveStatus_ReturnsSuccess()
        {
            ValidationResult result = _sut.Validate(StruttonTechnologies.Core.Identity.Validators.Identity.IdentityStatus.Active);

            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData(StruttonTechnologies.Core.Identity.Validators.Identity.IdentityStatus.Pending)]
        [InlineData(StruttonTechnologies.Core.Identity.Validators.Identity.IdentityStatus.Suspended)]
        [InlineData(StruttonTechnologies.Core.Identity.Validators.Identity.IdentityStatus.Locked)]
        public void Validate_WithNonActiveStatus_ReturnsInvalidIdentityStatusFailure(StruttonTechnologies.Core.Identity.Validators.Identity.IdentityStatus input)
        {
            ValidationResult result = _sut.Validate(input);

            Assert.False(result.IsValid);
            Assert.Equal("InvalidIdentityStatus", result.Code);
        }
    }
}
