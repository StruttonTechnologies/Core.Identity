using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Dtos.Authentication
{
    public record SignOutResultDto(
    bool Success,
    string? Message = null
)
    {
        public static SignOutResultDto SuccessResult() =>
            new(true, "Token revoked successfully.");

        public static SignOutResultDto Failure(string reason) =>
            new(false, reason);
    }
}
