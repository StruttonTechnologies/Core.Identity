using StruttonTechnologies.Core.ToolKit.Validation.Models;

namespace StruttonTechnologies.Core.Identity.Tests.Validators.Access
{
    /// <summary>
    /// Contains test scenarios for <see cref="StruttonTechnologies.Core.Identity.Validators.Access.SessionIdValidator"/>.
    /// </summary>
    public class SessionIdValidatorTests
    {
        private readonly StruttonTechnologies.Core.Identity.Validators.Access.SessionIdValidator _sut = new();

        [Fact]
        public void Validate_WithValidSessionId_ReturnsSuccess()
        {
            ValidationResult result = _sut.Validate(Guid.NewGuid().ToString());

            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Validate_WithMissingSessionId_ReturnsRequiredFailure(string input)
        {
            ValidationResult result = _sut.Validate(input);

            Assert.False(result.IsValid);
            Assert.Equal("Required", result.Code);
        }

        [Theory]
        [InlineData("not-a-guid")]
        [InlineData("00000000-0000-0000-0000-000000000000")]
        public void Validate_WithInvalidSessionId_ReturnsInvalidSessionIdFailure(string input)
        {
            ValidationResult result = _sut.Validate(input);

            Assert.False(result.IsValid);
            Assert.Equal("InvalidSessionId", result.Code);
        }
    }
}
