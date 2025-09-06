namespace ST.Core.Identity.Dtos.Authentication.Logins
{
    public record AddLoginRequestDto(string UserId, string LoginProvider, string ProviderKey);
}