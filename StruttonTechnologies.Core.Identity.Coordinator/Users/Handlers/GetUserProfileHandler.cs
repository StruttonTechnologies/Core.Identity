using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Users.Queries;
using StruttonTechnologies.Core.Identity.Dtos.Users;

namespace StruttonTechnologies.Core.Identity.Coordinator.Users.Handlers
{
    public class GetUserProfileHandler<TUser> : IRequestHandler<GetUserProfileQuery, UserProfileResult>
    where TUser : class
    {
        private readonly UserManager<TUser> _userManager;

        public GetUserProfileHandler(UserManager<TUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UserProfileResult> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            TUser? user = await _userManager.FindByIdAsync(request.UserId);

            ArgumentNullException.ThrowIfNull(user, $"User with ID '{request.UserId}' not found.");

            string userId = await _userManager.GetUserIdAsync(user);
            string? email = await _userManager.GetEmailAsync(user);
            string? displayName = await _userManager.GetUserNameAsync(user);
            bool isLockedOut = await _userManager.IsLockedOutAsync(user);

            return new UserProfileResult(
                userId?.ToString() ?? string.Empty,
                email ?? string.Empty,
                displayName ?? string.Empty, // or DisplayName if you have one
                await _userManager.IsLockedOutAsync(user));
        }
    }
}
