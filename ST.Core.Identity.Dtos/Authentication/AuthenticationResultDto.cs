using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Dtos.Authentication
{
    public record AuthenticationResultDto(
    bool Success,
    string Token,
    string? FailureReason = null
)
    {
        public static AuthenticationResultDto SuccessResult(string token) =>
            new(true, token);

        public static AuthenticationResultDto Failure(string reason) =>
            new(false, string.Empty, reason);
    }
}
