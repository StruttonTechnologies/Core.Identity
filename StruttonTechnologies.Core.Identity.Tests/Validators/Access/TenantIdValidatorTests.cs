using System.Diagnostics.CodeAnalysis;

using StruttonTechnologies.Core.ToolKit.Validation.Models;

namespace StruttonTechnologies.Core.Identity.Tests.Validators.Access
{
    /// <summary>
    /// Contains test scenarios for <see cref="StruttonTechnologies.Core.Identity.Validators.Access.TenantIdValidator"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class TenantIdValidatorTests
    {
        private readonly StruttonTechnologies.Core.Identity.Validators.Access.TenantIdValidator _sut = new();

        [Fact]
        public void Validate_WithValidTenantId_ReturnsSuccess()
        {
            ValidationResult result = _sut.Validate(Guid.NewGuid().ToString());

            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Validate_WithMissingTenantId_ReturnsMissingTenantIdFailure(string input)
        {
            ValidationResult result = _sut.Validate(input);

            Assert.False(result.IsValid);
            Assert.Equal("MissingTenantId", result.Code);
        }

        [Theory]
        [InlineData("not-a-guid")]
        [InlineData("00000000-0000-0000-0000-000000000000")]
        public void Validate_WithInvalidTenantId_ReturnsInvalidTenantIdFailure(string input)
        {
            ValidationResult result = _sut.Validate(input);

            Assert.False(result.IsValid);
            Assert.Equal("InvalidTenantId", result.Code);
        }
    }
}
