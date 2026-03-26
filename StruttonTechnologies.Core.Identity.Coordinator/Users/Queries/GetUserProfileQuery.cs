using MediatR;

using StruttonTechnologies.Core.Identity.Dtos.Users;

namespace StruttonTechnologies.Core.Identity.Coordinator.Users.Queries
{
    public class GetUserProfileQuery : IRequest<UserProfileDto>
    {
        public string UserId { get; }

        public GetUserProfileQuery(string userId)
        {
            UserId = userId;
        }
    }
}
