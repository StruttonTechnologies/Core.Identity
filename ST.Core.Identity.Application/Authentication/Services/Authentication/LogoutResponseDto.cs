namespace ST.Core.Identity.Application.Authentication.Services.Authentication
{
    /// <summary>
    /// Represents the result of a user logout operation.
    /// </summary>
    /// <param name="LoggedOutAt">The UTC timestamp when the logout occurred.</param>
    /// <param name="Message">A message describing the outcome of the logout.</param>
    public record LogoutResponseDto(
        DateTime LoggedOutAt,
        string Message
    );
}