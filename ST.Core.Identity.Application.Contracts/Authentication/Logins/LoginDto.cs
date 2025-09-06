namespace ST.Core.Identity.Application.Contracts.Authentication.Logins
{
    public record LoginDto(string UserId, string LoginProvider, string ProviderKey);
}