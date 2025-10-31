using Moq;
using ST.Core.Identity.Validators.Composite;
using ST.Core.Identity.Validators.Identity;
using ST.Core.Validators;
using ST.Core.Validators.Results.Models;

namespace ST.Core.Identity.EF.Tests.Validators
{
    public class AuthenticationContextValidatorTests
    {
        private readonly AuthenticationContextValidator _validator;

        public AuthenticationContextValidatorTests()
        {
            var providerValidator = new Mock<IValidator<string>>();
            providerValidator.Setup(v => v.Validate(It.IsAny<string>()))
                .Returns(ValidationResult.Success());

            var sessionValidator = new Mock<IValidator<string>>();
            sessionValidator.Setup(v => v.Validate(It.IsAny<string>()))
                .Returns(ValidationResult.Success());

            var statusValidator = new Mock<IValidator<IdentityStatus>>();
            statusValidator.Setup(v => v.Validate(It.IsAny<IdentityStatus>()))
                .Returns(ValidationResult.Success());

            _validator = new AuthenticationContextValidator(
                providerValidator.Object,
                sessionValidator.Object,
                statusValidator.Object);
        }

        public static IEnumerable<object[]> ValidContexts =>
            new[]
            {
                new object[]
                {
                    new AuthContext("Local", Guid.NewGuid().ToString(), IdentityStatus.Active)
                },
                new object[]
                {
                    new AuthContext("Google", Guid.NewGuid().ToString(), IdentityStatus.Pending)
                }
            };

        [Theory]
        [MemberData(nameof(ValidContexts))]
        public void Should_Return_Success_For_Valid_Contexts(AuthContext context)
        {
            var result = _validator.Validate(context);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void Should_Fail_When_Provider_Is_Invalid()
        {
            var providerValidator = new Mock<IValidator<string>>();
            providerValidator.Setup(v => v.Validate(It.IsAny<string>()))
                .Returns(ValidationResult.Failure("Invalid provider", "InvalidProvider", "Provider"));

            var validator = new AuthenticationContextValidator(
                providerValidator.Object,
                new Mock<IValidator<string>>().Object,
                new Mock<IValidator<IdentityStatus>>().Object);

            var context = new AuthContext("InvalidProvider", Guid.NewGuid().ToString(), IdentityStatus.Active);
            var result = validator.Validate(context);

            Assert.False(result.IsValid);
            Assert.Equal("InvalidProvider", result.Code);
        }
    }
}