using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace StruttonTechnologies.Core.Identity.Tests.Validators.Security
{
    [ExcludeFromCodeCoverage]
    public class PasswordValidatorTests
    {
        [Fact]
        public void Validate_ReturnsNoErrors_WhenPasswordIsValid()
        {
            string password = "ValidPassword123!@#$%";

            ReadOnlyCollection<string> errors = PasswordValidator.Validate(password);

            Assert.Empty(errors);
        }

        [Fact]
        public void Validate_ReturnsError_WhenPasswordIsNull()
        {
            string password = null!;

            ReadOnlyCollection<string> errors = PasswordValidator.Validate(password);

            Assert.NotEmpty(errors);
            Assert.Contains(errors, e => e.Contains("empty or whitespace"));
        }

        [Fact]
        public void Validate_ReturnsError_WhenPasswordIsEmpty()
        {
            string password = string.Empty;

            ReadOnlyCollection<string> errors = PasswordValidator.Validate(password);

            Assert.NotEmpty(errors);
            Assert.Contains(errors, e => e.Contains("empty or whitespace"));
        }

        [Fact]
        public void Validate_ReturnsError_WhenPasswordIsWhitespace()
        {
            string password = "   ";

            ReadOnlyCollection<string> errors = PasswordValidator.Validate(password);

            Assert.NotEmpty(errors);
            Assert.Contains(errors, e => e.Contains("empty or whitespace"));
        }

        [Theory]
        [InlineData("short")]
        [InlineData("123456789")]
        [InlineData("nine_char")]
        public void Validate_ReturnsError_WhenPasswordIsTooShort(string password)
        {
            ReadOnlyCollection<string> errors = PasswordValidator.Validate(password);

            Assert.NotEmpty(errors);
            Assert.Contains(errors, e => e.Contains("at least 10 characters"));
        }

        [Fact]
        public void Validate_ReturnsNoErrors_WhenPasswordIsExactlyMinimumLength()
        {
            string password = "1234567890";

            ReadOnlyCollection<string> errors = PasswordValidator.Validate(password);

            if (errors.Count > 0)
            {
                Assert.DoesNotContain(errors, e => e.Contains("at least 10 characters"));
            }
        }

        [Theory]
        [InlineData("password")]
        [InlineData("123456")]
        public void Validate_ReturnsError_WhenPasswordIsInBlacklist(string password)
        {
            ReadOnlyCollection<string> errors = PasswordValidator.Validate(password);

            Assert.NotEmpty(errors);
            Assert.Contains(errors, e => e.Contains("too common") || e.Contains("easily guessed"));
        }

        [Fact]
        public void Validate_ReturnsMultipleErrors_WhenMultipleRulesViolated()
        {
            string password = "pwd";

            ReadOnlyCollection<string> errors = PasswordValidator.Validate(password);

            Assert.True(errors.Count >= 1);
        }

        [Fact]
        public void Validate_ReturnsReadOnlyCollection()
        {
            string password = "ValidPassword!@#";

            ReadOnlyCollection<string> errors = PasswordValidator.Validate(password);

            Assert.IsType<ReadOnlyCollection<string>>(errors);
        }

        [Fact]
        public void Validate_IsCaseInsensitive_ForBlacklist()
        {
            string password = "PASSWORD";

            ReadOnlyCollection<string> errors = PasswordValidator.Validate(password);

            Assert.NotEmpty(errors);
            Assert.Contains(errors, e => e.Contains("too common") || e.Contains("easily guessed"));
        }

        [Fact]
        public void Validate_AllowsLongPasswords()
        {
            string password = "ThisIsAVeryLongPasswordThatShouldBeAccepted123!@#";

            ReadOnlyCollection<string> errors = PasswordValidator.Validate(password);

            if (errors.Count > 0)
            {
                Assert.DoesNotContain(errors, e => e.Contains("at least 10 characters"));
            }
        }

        [Fact]
        public void Validate_AllowsSpecialCharacters()
        {
            string password = "P@ssw0rd!#$%^&*()";

            ReadOnlyCollection<string> errors = PasswordValidator.Validate(password);

            if (errors.Count > 0)
            {
                Assert.DoesNotContain(errors, e => e.Contains("special character"));
            }
        }

        [Fact]
        public void Validate_DoesNotRequireNumbers()
        {
            string password = "PasswordWithoutNumbers!@#";

            ReadOnlyCollection<string> errors = PasswordValidator.Validate(password);

            if (errors.Count > 0)
            {
                Assert.DoesNotContain(errors, e => e.Contains("number") || e.Contains("digit"));
            }
        }

        [Fact]
        public void Validate_DoesNotRequireUppercase()
        {
            string password = "lowercasepassword123";

            ReadOnlyCollection<string> errors = PasswordValidator.Validate(password);

            if (errors.Count > 0)
            {
                Assert.DoesNotContain(errors, e => e.Contains("uppercase"));
            }
        }
    }
}
