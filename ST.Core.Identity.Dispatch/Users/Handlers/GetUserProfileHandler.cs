using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ST.Core.Identity.Dispatch.Users.Queries;
using ST.Core.Identity.Dtos.Users;
using ST.Core.Registration.Attributes;

namespace ST.Core.Identity.Dispatch.Users.Handlers
{
    [AutoRegister(ServiceLifetime.Scoped)]
    public class GetUserProfileHandler<TUser> : IRequestHandler<GetUserProfileQuery, UserProfileDto>
    where TUser : class
    {
        private readonly UserManager<TUser> _userManager;

        public GetUserProfileHandler(UserManager<TUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UserProfileDto> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
                return null;

            var userId = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var displayName = await _userManager.GetUserNameAsync(user);
            var isLockedOut = await _userManager.IsLockedOutAsync(user);

            return UserProfileDto.PopulateDto(
                userId,
                email,
                displayName,
                isLockedOut
            );
        }
    }
}
