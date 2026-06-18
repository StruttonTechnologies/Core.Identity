using StruttonTechnologies.Core.Identity.Coordinator.Authentication.Handlers;
using StruttonTechnologies.Core.Identity.Coordinator.Contracts.ExternalLogins.Commands;
using StruttonTechnologies.Core.Identity.Dtos.Authentication;
using StruttonTechnologies.Core.Identity.Orchestration.Contracts.ExternalLogins;
using StruttonTechnologies.Core.Identity.Stub.Entities;
using StruttonTechnologies.Core.Identity.Tests.Handlers.Base;

namespace StruttonTechnologies.Core.Identity.Tests.Handlers.Authentication.ExternalLogins;

[ExcludeFromCodeCoverage]
public class ExternalLoginCommandHandlerTests : CoordinatorHandlerTestBase
{
    [Fact]
    public async Task Handle_WhenValidatorReturnsIdentity_IssuesAccessAndRefreshTokens()
    {
        ExternalLoginCommand request = new("Google", "provider-token");

        ExternalLoginIdentityValidatorMock
            .Setup(x => x.ValidateAsync("Google", "provider-token", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ExternalLoginIdentity(
                "Google",
                "provider-key",
                TestUser.Email!,
                TestUser.DisplayName,
                new[] { new Claim("given_name", "Stub") }));

        UserManagerMock
            .Setup(x => x.FindByLoginAsync("Google", "provider-key"))
            .ReturnsAsync(TestUser);

        UserManagerMock
            .Setup(x => x.GetLoginsAsync(TestUser))
            .ReturnsAsync(new List<UserLoginInfo>
            {
            new("Google", "provider-key", "Google"),
            });

        UserManagerMock
            .Setup(x => x.GetRolesAsync(TestUser))
            .ReturnsAsync(new List<string>());

        UserManagerMock
            .Setup(x => x.GetEmailAsync(TestUser))
            .ReturnsAsync(TestUser.Email);

        TokenOrchestrationMock
            .Setup(x => x.GenerateTokenAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync("access-token");

        TokenOrchestrationMock
            .Setup(x => x.GenerateRefreshTokenAsync(
                TestUser.Id,
                "Stub User",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync("refresh-token");

        TokenOrchestrationMock
            .Setup(x => x.GetExpirationTime())
            .Returns(DateTime.UtcNow.AddMinutes(60));

        ExternalLoginCommandHandler<StubUser, Guid> sut =
            new(
                UserManagerMock.Object,
                ExternalLoginIdentityValidatorMock.Object,
                TokenOrchestrationMock.Object);

        TokenResponseDto result = await sut.Handle(request, CancellationToken.None);

        result.AccessToken.Should().Be("access-token");
        result.RefreshToken.Should().Be("refresh-token");

        TokenOrchestrationMock.Verify(
            x => x.GenerateTokenAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.IsAny<CancellationToken>()),
            Times.Once);

        TokenOrchestrationMock.Verify(
            x => x.GenerateRefreshTokenAsync(
                TestUser.Id,
                "Stub User",
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenValidatorReturnsNull_ThrowsInvalidOperationException()
    {
        ExternalLoginCommand request = new("Google", "provider-token");

        ExternalLoginIdentityValidatorMock
            .Setup(x => x.ValidateAsync("Google", "provider-token", It.IsAny<CancellationToken>()))
            .ReturnsAsync((ExternalLoginIdentity?)null);

        ExternalLoginCommandHandler<StubUser, Guid> sut = new Coordinator.Authentication.Handlers.ExternalLoginCommandHandler<StubUser, Guid>(
            UserManagerMock.Object,
            ExternalLoginIdentityValidatorMock.Object,
            TokenOrchestrationMock.Object);

        Func<Task> act = async () => await sut.Handle(request, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}
