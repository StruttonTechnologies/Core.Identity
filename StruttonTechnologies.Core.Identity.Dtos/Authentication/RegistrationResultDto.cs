namespace StruttonTechnologies.Core.Identity.Dtos.Authentication
{
    public record RegistrationResultDto(
    bool Success,
    string? UserId = null,
    string? FailureReason = null
)
    {
        public static RegistrationResultDto SuccessResult(string userId)
        {
            return new(true, userId);
        }

        public static RegistrationResultDto Failure(string reason)
        {
            return new(false, null, reason);
        }
    }
}
