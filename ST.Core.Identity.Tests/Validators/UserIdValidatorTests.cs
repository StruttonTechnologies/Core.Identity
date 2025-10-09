using ST.Core.Identity.Validators.Identity;

namespace ST.Core.Identity.Tests.Validators
{
    public class UserIdValidatorTests
    {
        private readonly UserIdValidator _validator = new();

        [Fact]
        public void Should_Return_Success_For_Valid_Guid()
        {
            var result = _validator.Validate(Guid.NewGuid());
            Assert.True(result.IsValid);
        }

        [Fact]
        public void Should_Return_Failure_For_Empty_Guid()
        {
            var result = _validator.Validate(Guid.Empty);
            Assert.False(result.IsValid);
            Assert.Equal("MissingUserId", result.Code);
        }
    }
}
