using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore.Identity;

using Moq;

using StruttonTechnologies.Core.Identity.Validators.Security;

namespace StruttonTechnologies.Core.Identity.Tests.Validators.Security
{
    [ExcludeFromCodeCoverage]
    public class WhitelistEmailValidatorTests
    {
        private readonly WhitelistEmailValidator<IdentityUser<Guid>> _validator;
        private readonly Mock<UserManager<IdentityUser<Guid>>> _mockUserManager;

        public WhitelistEmailValidatorTests()
        {
            _validator = new WhitelistEmailValidator<IdentityUser<Guid>>();
            _mockUserManager = CreateMockUserManager();
        }

        [Fact]
        public async Task ValidateAsync_ReturnsSuccess_WhenEmailIsNull()
        {
            IdentityUser<Guid> user = new IdentityUser<Guid>
            {
                Email = null
            };

            IdentityResult result = await _validator.ValidateAsync(_mockUserManager.Object, user);

            Assert.True(result.Succeeded);
        }

        [Fact]
        public async Task ValidateAsync_ReturnsSuccess_WhenEmailIsEmpty()
        {
            IdentityUser<Guid> user = new IdentityUser<Guid>
            {
                Email = string.Empty
            };

            IdentityResult result = await _validator.ValidateAsync(_mockUserManager.Object, user);

            Assert.True(result.Succeeded);
        }

        [Fact]
        public async Task ValidateAsync_ReturnsSuccess_WhenEmailIsWhitespace()
        {
            IdentityUser<Guid> user = new IdentityUser<Guid>
            {
                Email = "   "
            };

            IdentityResult result = await _validator.ValidateAsync(_mockUserManager.Object, user);

            Assert.True(result.Succeeded);
        }

        [Theory]
        [InlineData("admin@struttontechnologies.com")]
        [InlineData("support@struttontechnologies.com")]
        public async Task ValidateAsync_ReturnsSuccess_WhenEmailIsWhitelisted(string whitelistedEmail)
        {
            IdentityUser<Guid> user = new IdentityUser<Guid>
            {
                Email = whitelistedEmail
            };

            IdentityResult result = await _validator.ValidateAsync(_mockUserManager.Object, user);

            Assert.True(result.Succeeded);
        }

        [Theory]
        [InlineData("ADMIN@STRUTTONTECHNOLOGIES.COM")]
        [InlineData("Support@StruttonTechnologies.Com")]
        public async Task ValidateAsync_IsCaseInsensitive_ForWhitelistedEmails(string whitelistedEmail)
        {
            IdentityUser<Guid> user = new IdentityUser<Guid>
            {
                Email = whitelistedEmail
            };

            IdentityResult result = await _validator.ValidateAsync(_mockUserManager.Object, user);

            Assert.True(result.Succeeded);
        }

        [Theory]
        [InlineData("user@struttontechnologies.com")]
        [InlineData("test@example.com")]
        public async Task ValidateAsync_ReturnsSuccess_WhenDomainIsWhitelisted(string email)
        {
            IdentityUser<Guid> user = new IdentityUser<Guid>
            {
                Email = email
            };

            IdentityResult result = await _validator.ValidateAsync(_mockUserManager.Object, user);

            Assert.True(result.Succeeded);
        }

        [Theory]
        [InlineData("user@STRUTTONTECHNOLOGIES.COM")]
        [InlineData("test@EXAMPLE.COM")]
        public async Task ValidateAsync_IsCaseInsensitive_ForWhitelistedDomains(string email)
        {
            IdentityUser<Guid> user = new IdentityUser<Guid>
            {
                Email = email
            };

            IdentityResult result = await _validator.ValidateAsync(_mockUserManager.Object, user);

            Assert.True(result.Succeeded);
        }

        [Theory]
        [InlineData("user@notallowed.com")]
        [InlineData("test@badactor.org")]
        public async Task ValidateAsync_ReturnsFailure_WhenEmailIsNotWhitelisted(string email)
        {
            IdentityUser<Guid> user = new IdentityUser<Guid>
            {
                Email = email
            };

            IdentityResult result = await _validator.ValidateAsync(_mockUserManager.Object, user);

            Assert.False(result.Succeeded);
            Assert.Single(result.Errors);
            Assert.Equal("EmailNotWhitelisted", result.Errors.First().Code);
            Assert.Equal("Email address is not allowed.", result.Errors.First().Description);
        }

        [Fact]
        public async Task ValidateAsync_ReturnsFailure_WhenEmailHasNoAtSign()
        {
            IdentityUser<Guid> user = new IdentityUser<Guid>
            {
                Email = "invalidemail"
            };

            IdentityResult result = await _validator.ValidateAsync(_mockUserManager.Object, user);

            Assert.False(result.Succeeded);
        }

        [Fact]
        public async Task ValidateAsync_ReturnsSuccess_WhenUserIsNull()
        {
            IdentityResult result = await _validator.ValidateAsync(_mockUserManager.Object, null!);

            Assert.True(result.Succeeded);
        }

        private static Mock<UserManager<IdentityUser<Guid>>> CreateMockUserManager()
        {
            Mock<IUserStore<IdentityUser<Guid>>> storeMock = new Mock<IUserStore<IdentityUser<Guid>>>();
            return new Mock<UserManager<IdentityUser<Guid>>>(
                storeMock.Object, null, null, null, null, null, null, null, null);
        }
    }
}
