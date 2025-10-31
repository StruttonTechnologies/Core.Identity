using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Dtos.Users
{
    public record UserProfileDto(
    string Id,
    string? Email,
    string? DisplayName,
    bool IsActive
)
    {
        public static UserProfileDto PopulateDto(
            string userId,
            string? email,
            string? displayName,
            bool isLockedOut)
        {
            return new UserProfileDto(
                Id: userId,
                Email: email ?? string.Empty,
                DisplayName: displayName ?? string.Empty,
                IsActive: !isLockedOut
            );
        }
    }
}
