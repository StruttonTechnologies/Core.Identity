namespace ST.Core.Identity.Dtos.Authentication.Logins
{
    public record RemoveLoginRequestDto(string UserId, string LoginProvider, string ProviderKey);
}