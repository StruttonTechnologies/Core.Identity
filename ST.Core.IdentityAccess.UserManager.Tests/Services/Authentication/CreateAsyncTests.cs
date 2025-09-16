using ST.Core.Identity.Fakes.Factories;
using ST.Core.IdentityAccess.Fakes.UserManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.IdentityAccess.UserManager.Tests.Services.Authentication
{
    public class CreateAsyncTests : AuthenticationUserServiceTestBase
    {
        [Fact]
        public async Task CreateAsync_Success_ReturnsUser()
        {
            var user = TestAppUserIdentityFactory.CreateDefault();
            var password = "SecureP@ssword123";

            var result = await Service.CreateAsync(user, password);

            Assert.Equal(user, result);
        }

        [Theory]
        [InlineData(null, "ValidPassword", typeof(ArgumentNullException))]
        [InlineData("ValidUser", "", typeof(ArgumentException))]
        public async Task CreateAsync_InvalidArguments_ThrowsExpectedException(string? userName, string password, Type expectedException)
        {
            var user = userName == null ? null! : TestAppUserIdentityFactory.Create(userName);

            var exception = await Record.ExceptionAsync(() => Service.CreateAsync(user, password));

            Assert.NotNull(exception);
            Assert.IsType(expectedException, exception);
        }
    }
}
