using MediatR;

using StruttonTechnologies.Core.Identity.Dtos.Users;

namespace StruttonTechnologies.Core.Identity.Coordinator.Contracts.Users.Queries
{
    public class GetUserProfileQuery : IRequest<UserProfileResult>
    {
        public string UserId { get; }

        public GetUserProfileQuery(string userId)
        {
            UserId = userId;
        }
    }
}
