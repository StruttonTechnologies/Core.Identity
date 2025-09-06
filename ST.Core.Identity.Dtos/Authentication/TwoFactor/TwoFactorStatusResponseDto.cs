namespace ST.Core.Identity.Dtos.Authentication.TwoFactor
{
    public record TwoFactorStatusResponseDto(string UserId, bool Enabled, IEnumerable<string> Providers);
}