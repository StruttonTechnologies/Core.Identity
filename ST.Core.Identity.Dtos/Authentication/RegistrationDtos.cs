namespace ST.Core.Identity.Dtos.Authentication
{
    public record RegistrationRequestDto(string UserName, string Email, string Password);

    public record RegistrationResponseDto(Guid UserId, string UserName, bool EmailConfirmed);
}