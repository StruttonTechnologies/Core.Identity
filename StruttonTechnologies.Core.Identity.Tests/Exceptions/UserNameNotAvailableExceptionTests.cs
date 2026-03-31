using System.Diagnostics.CodeAnalysis;

using StruttonTechnologies.Core.Identity.Exceptions;

namespace StruttonTechnologies.Core.Identity.Tests.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class UserNameNotAvailableExceptionTests
    {
        [Fact]
        public void Constructor_Default_CreatesException()
        {
            UserNameNotAvailableException exception = new UserNameNotAvailableException();

            Assert.NotNull(exception);
            Assert.IsAssignableFrom<InvalidOperationException>(exception);
        }

        [Fact]
        public void Constructor_WithUserName_SetsMessage()
        {
            string userName = "testuser";

            UserNameNotAvailableException exception = new UserNameNotAvailableException(userName);

            Assert.NotNull(exception);
            Assert.Contains(userName, exception.Message);
            Assert.Contains("exists", exception.Message);
        }

        [Fact]
        public void Constructor_WithMessageAndInnerException_SetsProperties()
        {
            string message = "Test message";
            Exception innerException = new InvalidOperationException("Inner");

            UserNameNotAvailableException exception = new UserNameNotAvailableException(message, innerException);

            Assert.Equal(message, exception.Message);
            Assert.Same(innerException, exception.InnerException);
        }

        [Fact]
        public void ThrowIfExists_ThrowsException_WhenUserExists()
        {
            TestUser user = new TestUser { UserName = "existinguser" };
            string userName = "existinguser";

            UserNameNotAvailableException exception = Assert.Throws<UserNameNotAvailableException>(() =>
                UserNameNotAvailableException.ThrowIfExists(user, userName));

            Assert.Contains(userName, exception.Message);
        }

        [Fact]
        public void ThrowIfExists_DoesNotThrow_WhenUserIsNull()
        {
            TestUser? user = null;
            string userName = "newuser";

            UserNameNotAvailableException.ThrowIfExists(user, userName);
        }

        [Fact]
        public void ThrowIfExists_WorksWithDifferentUserTypes()
        {
            CustomUser user = new CustomUser { Id = 1 };
            string userName = "customuser";

            UserNameNotAvailableException exception = Assert.Throws<UserNameNotAvailableException>(() =>
                UserNameNotAvailableException.ThrowIfExists(user, userName));

            Assert.NotNull(exception);
        }

        private class TestUser
        {
            public string UserName { get; set; } = string.Empty;
        }

        private class CustomUser
        {
            public int Id { get; set; }
        }
    }
}
