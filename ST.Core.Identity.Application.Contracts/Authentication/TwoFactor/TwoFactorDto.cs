namespace ST.Core.Identity.Application.Contracts.Authentication.TwoFactor
{
    public record TwoFactorDto(string UserId, string Provider, string Token, bool? Enabled = null);
}