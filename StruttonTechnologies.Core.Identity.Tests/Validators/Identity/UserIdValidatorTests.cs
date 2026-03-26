using StruttonTechnologies.Core.ToolKit.Validation.Models;

namespace StruttonTechnologies.Core.Identity.Tests.Validators.Identity
{
    /// <summary>
    /// Contains test scenarios for <see cref="StruttonTechnologies.Core.Identity.Validators.Identity.UserIdValidator"/>.
    /// </summary>
    public class UserIdValidatorTests
    {
        private readonly StruttonTechnologies.Core.Identity.Validators.Identity.UserIdValidator _sut = new();

        [Fact]
        public void Validate_WithUserId_ReturnsSuccess()
        {
            ValidationResult result = _sut.Validate(Guid.NewGuid());

            Assert.True(result.IsValid);
        }

        [Fact]
        public void Validate_WithoutUserId_ReturnsMissingUserIdFailure()
        {
            ValidationResult result = _sut.Validate(Guid.Empty);

            Assert.False(result.IsValid);
            Assert.Equal("MissingUserId", result.Code);
        }
    }
}
