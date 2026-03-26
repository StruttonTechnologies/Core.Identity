namespace StruttonTechnologies.Core.Identity.Dtos.Authentication
{
    public record AuthenticateRequestDto(
        string Email,
        string Password);
}
