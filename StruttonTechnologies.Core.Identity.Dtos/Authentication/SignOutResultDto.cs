namespace StruttonTechnologies.Core.Identity.Dtos.Authentication
{
    public record SignOutResultDto(
    bool Success,
    string? Message = null
)
    {
        public static SignOutResultDto SuccessResult()
        {
            return new(true, "Token revoked successfully.");
        }

        public static SignOutResultDto Failure(string reason)
        {
            return new(false, reason);
        }
    }
}
