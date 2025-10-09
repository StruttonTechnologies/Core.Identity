using ST.Core.Identity.Validators.Identity;

namespace ST.Core.Identity.Tests.Validators
{
    public class IdentityStatusValidatorTests
    {
        private readonly IdentityStatusValidator _validator = new();

        [Fact]
        public void Should_Return_Success_For_Active_Status()
        {
            var result = _validator.Validate(IdentityStatus.Active);
            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData(IdentityStatus.Suspended)]
        [InlineData(IdentityStatus.Pending)]
        [InlineData(IdentityStatus.Deactivated)]
        public void Should_Return_Failure_For_Inactive_Status(IdentityStatus status)
        {
            var result = _validator.Validate(status);
            Assert.False(result.IsValid);
            Assert.Equal("InvalidIdentityStatus", result.Code);
        }
    }
}
