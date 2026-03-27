using System.Diagnostics.CodeAnalysis;

using Moq;

using StruttonTechnologies.Core.Identity.Validators.Composite;
using StruttonTechnologies.Core.Identity.Validators.Identity;
using StruttonTechnologies.Core.ToolKit.Validation.Abstractions;
using StruttonTechnologies.Core.ToolKit.Validation.Models;

namespace StruttonTechnologies.Core.Identity.EF.Tests.Validators
{
    [ExcludeFromCodeCoverage]
    public class AuthenticationContextValidatorTests
    {
        private readonly AuthenticationContextValidator _validator;

        public AuthenticationContextValidatorTests()
        {
            Mock<IValidator<string>> providerValidator = new Mock<IValidator<string>>();
            providerValidator.Setup(v => v.Validate(It.IsAny<string>()))
                .Returns(ValidationResult.Success());

            Mock<IValidator<string>> sessionValidator = new Mock<IValidator<string>>();
            sessionValidator.Setup(v => v.Validate(It.IsAny<string>()))
                .Returns(ValidationResult.Success());

            Mock<IValidator<IdentityStatus>> statusValidator = new Mock<IValidator<IdentityStatus>>();
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
            ValidationResult result = _validator.Validate(context);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void Should_Fail_When_Provider_Is_Invalid()
        {
            Mock<IValidator<string>> providerValidator = new Mock<IValidator<string>>();
            providerValidator.Setup(v => v.Validate(It.IsAny<string>()))
                .Returns(ValidationResult.Failure("Invalid provider", "InvalidProvider", "Provider"));

            AuthenticationContextValidator validator = new AuthenticationContextValidator(
                providerValidator.Object,
                new Mock<IValidator<string>>().Object,
                new Mock<IValidator<IdentityStatus>>().Object);

            AuthContext context = new AuthContext("InvalidProvider", Guid.NewGuid().ToString(), IdentityStatus.Active);
            ValidationResult result = validator.Validate(context);

            Assert.False(result.IsValid);
            Assert.Equal("InvalidProvider", result.Code);
        }
    }
}
