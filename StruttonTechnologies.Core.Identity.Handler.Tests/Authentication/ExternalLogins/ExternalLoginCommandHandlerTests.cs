using StruttonTechnologies.Core.Identity.Coordinator.Contracts.ExternalLogins.Commands;
using StruttonTechnologies.Core.Identity.Handler.Tests.Base;
using StruttonTechnologies.Core.Identity.Orchestration.Contracts.ExternalLogins;
using StruttonTechnologies.Core.Identity.Stub.Entities;

namespace StruttonTechnologies.Core.Identity.Handler.Tests.Authentication.ExternalLogins
{
    [ExcludeFromCodeCoverage]
    public class ExternalLoginCommandHandlerTests : CoordinatorHandlerTestBase
    {
        [Fact]
        public async Task Handle_WhenValidatorReturnsIdentity_IssuesAccessToken()
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

            TokenOrchestrationMock
                .Setup(x => x.GenerateTokenAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("access-token");

            TokenOrchestrationMock
                .Setup(x => x.GetExpirationTime())
                .Returns(DateTime.UtcNow.AddMinutes(60));

            var sut = new StruttonTechnologies.Core.Identity.Coordinator.Authentication.Handlers.ExternalLoginCommandHandler<StubUser, Guid>(
                UserManagerMock.Object,
                ExternalLoginIdentityValidatorMock.Object,
                TokenOrchestrationMock.Object);

            var result = await sut.Handle(request, CancellationToken.None);

            result.AccessToken.Should().Be("access-token");
            result.RefreshToken.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_WhenValidatorReturnsNull_ThrowsInvalidOperationException()
        {
            ExternalLoginCommand request = new("Google", "provider-token");

            ExternalLoginIdentityValidatorMock
                .Setup(x => x.ValidateAsync("Google", "provider-token", It.IsAny<CancellationToken>()))
                .ReturnsAsync((ExternalLoginIdentity?)null);

            var sut = new StruttonTechnologies.Core.Identity.Coordinator.Authentication.Handlers.ExternalLoginCommandHandler<StubUser, Guid>(
                UserManagerMock.Object,
                ExternalLoginIdentityValidatorMock.Object,
                TokenOrchestrationMock.Object);

            Func<Task> act = async () => await sut.Handle(request, CancellationToken.None);

            await act.Should().ThrowAsync<InvalidOperationException>();
        }
    }
}
