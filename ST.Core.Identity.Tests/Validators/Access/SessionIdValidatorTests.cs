using ST.Core.Identity.Validators.Access;
using Xunit;

namespace ST.Core.Identity.Tests.Validators.Access
{
    public class SessionIdValidatorTests
    {
        private readonly SessionIdValidator _validator = new();

        [Fact]
        public void Should_Return_Success_For_Valid_Guid()
        {
            var result = _validator.Validate(Guid.NewGuid().ToString());
            Assert.True(result.IsSuccess);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("not-a-guid")]
        [InlineData("00000000-0000-0000-0000-000000000000")]
        public void Should_Return_Failure_For_Invalid_Or_Missing_SessionId(string input)
        {
            var result = _validator.Validate(input);
            Assert.False(result.IsSuccess);
        }
    }
}