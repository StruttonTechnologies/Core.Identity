using ST.Core.Identity.Validators.Access;
using Xunit;
using System;

namespace ST.Core.Identity.Tests.Validators.Access
{
    public class TenantIdValidatorTests
    {
        private readonly TenantIdValidator _validator = new();

        [Fact]
        public void Should_Return_Success_For_Valid_TenantId()
        {
            var result = _validator.Validate(Guid.NewGuid().ToString());
            Assert.True(result.IsValid);
        }

        [Fact]
        public void Should_Return_Failure_For_Empty_Guid()
        {
            var result = _validator.Validate(Guid.Empty.ToString());
            Assert.False(result.IsValid);
            Assert.Equal("InvalidTenantId", result.Code);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void Should_Return_Failure_For_Missing_TenantId(string input)
        {
            var result = _validator.Validate(input);
            Assert.False(result.IsValid);
            Assert.Equal("MissingTenantId", result.Code);
        }
    }
}