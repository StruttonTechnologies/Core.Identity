namespace StruttonTechnologies.Core.Identity.Dtos.Authentication
{
    public record RegisterUserRequestDto(
        string Email,
        string Password,
        string DisplayName);
}
