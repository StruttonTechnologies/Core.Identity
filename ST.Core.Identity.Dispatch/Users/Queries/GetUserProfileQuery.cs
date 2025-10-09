using MediatR;
using ST.Core.Identity.Dtos.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Dispatch.Users.Queries
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
