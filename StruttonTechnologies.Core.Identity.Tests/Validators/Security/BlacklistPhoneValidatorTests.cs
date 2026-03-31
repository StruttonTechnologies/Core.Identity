using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore.Identity;

using Moq;

using StruttonTechnologies.Core.Identity.Validators.Security;

namespace StruttonTechnologies.Core.Identity.Tests.Validators.Security
{
    [ExcludeFromCodeCoverage]
    public class BlacklistPhoneValidatorTests
    {
        private readonly BlacklistPhoneValidator<IdentityUser<Guid>> _validator;
        private readonly Mock<UserManager<IdentityUser<Guid>>> _mockUserManager;

        public BlacklistPhoneValidatorTests()
        {
            _validator = new BlacklistPhoneValidator<IdentityUser<Guid>>();
            _mockUserManager = CreateMockUserManager();
        }

        [Fact]
        public async Task ValidateAsync_ReturnsSuccess_WhenPhoneNumberIsNull()
        {
            IdentityUser<Guid> user = new IdentityUser<Guid>
            {
                PhoneNumber = null
            };

            IdentityResult result = await _validator.ValidateAsync(_mockUserManager.Object, user);

            Assert.True(result.Succeeded);
        }

        [Fact]
        public async Task ValidateAsync_ReturnsSuccess_WhenPhoneNumberIsEmpty()
        {
            IdentityUser<Guid> user = new IdentityUser<Guid>
            {
                PhoneNumber = string.Empty
            };

            IdentityResult result = await _validator.ValidateAsync(_mockUserManager.Object, user);

            Assert.True(result.Succeeded);
        }

        [Fact]
        public async Task ValidateAsync_ReturnsSuccess_WhenPhoneNumberIsWhitespace()
        {
            IdentityUser<Guid> user = new IdentityUser<Guid>
            {
                PhoneNumber = "   "
            };

            IdentityResult result = await _validator.ValidateAsync(_mockUserManager.Object, user);

            Assert.True(result.Succeeded);
        }

        [Fact]
        public async Task ValidateAsync_ReturnsSuccess_WhenPhoneNumberIsNotBlacklisted()
        {
            IdentityUser<Guid> user = new IdentityUser<Guid>
            {
                PhoneNumber = "+15551234567"
            };

            IdentityResult result = await _validator.ValidateAsync(_mockUserManager.Object, user);

            Assert.True(result.Succeeded);
        }

        [Theory]
        [InlineData("+10000000000")]
        [InlineData("+19999999999")]
        [InlineData("+18001234567")]
        public async Task ValidateAsync_ReturnsFailure_WhenPhoneNumberIsBlacklisted(string blacklistedNumber)
        {
            IdentityUser<Guid> user = new IdentityUser<Guid>
            {
                PhoneNumber = blacklistedNumber
            };

            IdentityResult result = await _validator.ValidateAsync(_mockUserManager.Object, user);

            Assert.False(result.Succeeded);
            Assert.Single(result.Errors);
            Assert.Equal("PhoneNumberBlacklisted", result.Errors.First().Code);
            Assert.Equal("Phone number is not allowed.", result.Errors.First().Description);
        }

        [Theory]
        [InlineData("+10000000000")]
        [InlineData("+10000000000")]
        [InlineData("+10000000000")]
        public async Task ValidateAsync_IsCaseInsensitive(string blacklistedNumber)
        {
            IdentityUser<Guid> user = new IdentityUser<Guid>
            {
                PhoneNumber = blacklistedNumber.ToLowerInvariant()
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
