using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Dtos.Authentication
{
    public record RegistrationResultDto(
    bool Success,
    string? UserId = null,
    string? FailureReason = null
)
    {
        public static RegistrationResultDto SuccessResult(string userId) =>
            new(true, userId);

        public static RegistrationResultDto Failure(string reason) =>
            new(false, null, reason);
    }

}
