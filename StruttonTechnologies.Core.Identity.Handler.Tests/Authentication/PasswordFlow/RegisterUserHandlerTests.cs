using StruttonTechnologies.Core.Identity.Coordinator.Authentication.Handlers;
using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authentication.Commands;
using StruttonTechnologies.Core.Identity.Dtos.Authentication;
using StruttonTechnologies.Core.Identity.Handler.Tests.Base;
using StruttonTechnologies.Core.Identity.Stub.Entities;

namespace StruttonTechnologies.Core.Identity.Handler.Tests.Authentication.PasswordFlow
{
    [ExcludeFromCodeCoverage]
    public class RegisterUserHandlerTests : CoordinatorHandlerTestBase
    {
        [Fact]
        public async Task Handle_WhenCreateSucceeds_ReturnsSuccessfulRegistration()
        {
            RegisterUserCommand request = new("new.user@example.com", "Password123!", "New User");
            StubUser? captured = null;

            UserManagerMock.Setup(x => x.SetUserNameAsync(It.IsAny<StubUser>(), request.DisplayName)).Callback<StubUser, string>((u, _) => captured = u).ReturnsAsync(IdentityResult.Success);
            UserManagerMock.Setup(x => x.SetEmailAsync(It.IsAny<StubUser>(), request.Email)).ReturnsAsync(IdentityResult.Success);
            UserManagerMock.Setup(x => x.CreateAsync(It.IsAny<StubUser>(), request.Password)).ReturnsAsync(IdentityResult.Success);
            UserManagerMock.Setup(x => x.GetUserIdAsync(It.IsAny<StubUser>())).ReturnsAsync("created-user-id");

            RegisterUserHandler<StubUser> sut = new(UserManagerMock.Object);

            RegistrationResultDto result = await sut.Handle(request, CancellationToken.None);

            result.Success.Should().BeTrue();
            result.UserId.Should().Be("created-user-id");
            captured.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_WhenCreateFails_ReturnsFailureReason()
        {
            RegisterUserCommand request = new("new.user@example.com", "Password123!", "New User");
            UserManagerMock.Setup(x => x.SetUserNameAsync(It.IsAny<StubUser>(), request.DisplayName)).ReturnsAsync(IdentityResult.Success);
            UserManagerMock.Setup(x => x.SetEmailAsync(It.IsAny<StubUser>(), request.Email)).ReturnsAsync(IdentityResult.Success);
            UserManagerMock.Setup(x => x.CreateAsync(It.IsAny<StubUser>(), request.Password)).ReturnsAsync(Failed("Registration failed badly."));

            RegisterUserHandler<StubUser> sut = new(UserManagerMock.Object);

            RegistrationResultDto result = await sut.Handle(request, CancellationToken.None);

            result.Success.Should().BeFalse();
            result.FailureReason.Should().Be("Registration failed badly.");
        }
    }
}
