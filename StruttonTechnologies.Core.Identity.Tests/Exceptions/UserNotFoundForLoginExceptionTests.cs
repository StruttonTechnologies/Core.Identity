using System.Diagnostics.CodeAnalysis;

using StruttonTechnologies.Core.Identity.Exceptions;

namespace StruttonTechnologies.Core.Identity.Tests.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class UserNotFoundForLoginExceptionTests
    {
        [Fact]
        public void Constructor_Default_CreatesExceptionWithEmptyUserName()
        {
            UserNotFoundForLoginException exception = new UserNotFoundForLoginException();

            Assert.NotNull(exception);
            Assert.Equal(string.Empty, exception.UserName);
        }

        [Fact]
        public void Constructor_WithUserName_SetsUserNameAndMessage()
        {
            string userName = "testuser";

            UserNotFoundForLoginException exception = new UserNotFoundForLoginException(userName);

            Assert.Equal(userName, exception.UserName);
            Assert.Contains(userName, exception.Message);
            Assert.Contains("No user found", exception.Message);
            Assert.Contains("Registration required", exception.Message);
        }

        [Fact]
        public void Constructor_WithMessageAndInnerException_SetsPropertiesAndEmptyUserName()
        {
            string message = "Test message";
            Exception innerException = new InvalidOperationException("Inner");

            UserNotFoundForLoginException exception = new UserNotFoundForLoginException(message, innerException);

            Assert.Equal(message, exception.Message);
            Assert.Same(innerException, exception.InnerException);
            Assert.Equal(string.Empty, exception.UserName);
        }

        [Fact]
        public void Constructor_WithEmptyUserName_SetsEmptyUserName()
        {
            string userName = string.Empty;

            UserNotFoundForLoginException exception = new UserNotFoundForLoginException(userName);

            Assert.Equal(string.Empty, exception.UserName);
        }

        [Fact]
        public void Constructor_WithNullUserName_HandlesNull()
        {
            string userName = null!;

            UserNotFoundForLoginException exception = new UserNotFoundForLoginException(userName);

            Assert.Null(exception.UserName);
        }

        [Fact]
        public void UserName_IsReadOnly()
        {
            UserNotFoundForLoginException exception = new UserNotFoundForLoginException("testuser");

            Assert.Equal("testuser", exception.UserName);
        }

        [Fact]
        public void Exception_InheritsFromException()
        {
            UserNotFoundForLoginException exception = new UserNotFoundForLoginException();

            Assert.IsAssignableFrom<Exception>(exception);
        }
    }
}
