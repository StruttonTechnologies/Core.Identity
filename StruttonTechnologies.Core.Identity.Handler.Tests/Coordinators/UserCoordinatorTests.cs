using MediatR;

using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authorization.Queries;
using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Users.Queries;
using StruttonTechnologies.Core.Identity.Coordinator.Users.Coordinator;
using StruttonTechnologies.Core.Identity.Dtos.Authorization;
using StruttonTechnologies.Core.Identity.Dtos.Users;

namespace StruttonTechnologies.Core.Identity.Handler.Tests.Coordinators
{
    [ExcludeFromCodeCoverage]
    public class UserCoordinatorTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly UserCoordinator _sut;

        public UserCoordinatorTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _sut = new UserCoordinator(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetClaimsPrincipalAsync_SendsQuery_ReturnsResult()
        {
            string userId = Guid.NewGuid().ToString();
            List<ClaimDto> claims = new()
            {
                new ClaimDto("sub", userId)
            };
            ClaimsPrincipalDto expectedDto = new(
                AuthenticationType: "TestAuth",
                IsAuthenticated: true,
                Name: "test@example.com",
                Claims: claims
            );

            _mediatorMock.Setup(x => x.Send(It.Is<GetClaimsPrincipalQuery>(q => q.UserId == userId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedDto);

            ClaimsPrincipalDto? result = await _sut.GetClaimsPrincipalAsync(userId);

            result.Should().NotBeNull();
            result.Should().BeSameAs(expectedDto);
            _mediatorMock.Verify(x => x.Send(It.Is<GetClaimsPrincipalQuery>(q => q.UserId == userId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetClaimsPrincipalAsync_WhenMediatorReturnsNull_ReturnsNull()
        {
            string userId = Guid.NewGuid().ToString();

            _mediatorMock.Setup(x => x.Send(It.Is<GetClaimsPrincipalQuery>(q => q.UserId == userId), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ClaimsPrincipalDto?)null);

            ClaimsPrincipalDto? result = await _sut.GetClaimsPrincipalAsync(userId);

            result.Should().BeNull();
            _mediatorMock.Verify(x => x.Send(It.Is<GetClaimsPrincipalQuery>(q => q.UserId == userId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetNormalizedEmailAsync_SendsQuery_ReturnsResult()
        {
            string userId = Guid.NewGuid().ToString();
            string expectedEmail = "TEST@EXAMPLE.COM";

            _mediatorMock.Setup(x => x.Send(It.Is<GetNormalizedEmailQuery>(q => q.UserId == userId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedEmail);

            string? result = await _sut.GetNormalizedEmailAsync(userId);

            result.Should().Be(expectedEmail);
            _mediatorMock.Verify(x => x.Send(It.Is<GetNormalizedEmailQuery>(q => q.UserId == userId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetNormalizedEmailAsync_WhenMediatorReturnsNull_ReturnsNull()
        {
            string userId = Guid.NewGuid().ToString();

            _mediatorMock.Setup(x => x.Send(It.Is<GetNormalizedEmailQuery>(q => q.UserId == userId), It.IsAny<CancellationToken>()))
                .ReturnsAsync((string?)null);

            string? result = await _sut.GetNormalizedEmailAsync(userId);

            result.Should().BeNull();
            _mediatorMock.Verify(x => x.Send(It.Is<GetNormalizedEmailQuery>(q => q.UserId == userId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetUserRolesAsync_SendsQuery_ReturnsReadOnlyList()
        {
            string userId = Guid.NewGuid().ToString();
            IList<string> roles = new List<string> { "Admin", "User" };

            _mediatorMock.Setup(x => x.Send(It.Is<GetUserRolesQuery>(q => q.UserId == userId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(roles);

            IReadOnlyList<string> result = await _sut.GetUserRolesAsync(userId);

            result.Should().HaveCount(2);
            result.Should().Contain("Admin");
            result.Should().Contain("User");
            _mediatorMock.Verify(x => x.Send(It.Is<GetUserRolesQuery>(q => q.UserId == userId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetUserRolesAsync_WhenMediatorReturnsReadOnlyList_ReturnsSameInstance()
        {
            string userId = Guid.NewGuid().ToString();
            IList<string> roles = new List<string> { "Admin" }.AsReadOnly();

            _mediatorMock.Setup(x => x.Send(It.Is<GetUserRolesQuery>(q => q.UserId == userId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(roles);

            IReadOnlyList<string> result = await _sut.GetUserRolesAsync(userId);

            result.Should().BeSameAs(roles);
            _mediatorMock.Verify(x => x.Send(It.Is<GetUserRolesQuery>(q => q.UserId == userId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetUserRolesAsync_WhenMediatorReturnsEmptyList_ReturnsEmptyReadOnlyList()
        {
            string userId = Guid.NewGuid().ToString();
            IList<string> roles = new List<string>();

            _mediatorMock.Setup(x => x.Send(It.Is<GetUserRolesQuery>(q => q.UserId == userId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(roles);

            IReadOnlyList<string> result = await _sut.GetUserRolesAsync(userId);

            result.Should().BeEmpty();
            _mediatorMock.Verify(x => x.Send(It.Is<GetUserRolesQuery>(q => q.UserId == userId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetUserProfileAsync_SendsQuery_ReturnsResult()
        {
            string userId = Guid.NewGuid().ToString();
            UserProfileResult expectedProfile = new(
                UserId: userId,
                Email: "test@example.com",
                DisplayName: "Test User",
                IsLockedOut: false
            );

            _mediatorMock.Setup(x => x.Send(It.Is<GetUserProfileQuery>(q => q.UserId == userId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedProfile);

            UserProfileResult result = await _sut.GetUserProfileAsync(userId);

            result.Should().NotBeNull();
            result.Should().BeSameAs(expectedProfile);
            result.UserId.Should().Be(userId);
            _mediatorMock.Verify(x => x.Send(It.Is<GetUserProfileQuery>(q => q.UserId == userId), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
