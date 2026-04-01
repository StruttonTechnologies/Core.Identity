using System.Diagnostics.CodeAnalysis;

namespace StruttonTechnologies.Core.Identity.Dtos.Authentication
{
    [ExcludeFromCodeCoverage]

    public record AuthenticationResultDto(
        bool IsSuccess,
        string Token,
        string? FailureReason = null)
    {
        public static AuthenticationResultDto SuccessResult(string token)
        {
            return new(true, token);
        }

        public static AuthenticationResultDto Failure(string reason)
        {
            return new(false, string.Empty, reason);
        }

        public bool IsFailure => !IsSuccess;
    }
}
