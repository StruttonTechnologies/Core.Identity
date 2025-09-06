namespace ST.Core.Identity.Dtos.Authentication.Logins
{
    public record LoginInfoResponseDto(string UserId, string LoginProvider, string ProviderKey);
}