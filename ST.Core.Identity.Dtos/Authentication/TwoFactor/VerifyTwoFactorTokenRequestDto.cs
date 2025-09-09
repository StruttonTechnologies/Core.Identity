using System.Diagnostics.CodeAnalysis;

namespace ST.Core.Identity.Dtos.Authentication.TwoFactor
{
    [ExcludeFromCodeCoverage]
    public record VerifyTwoFactorTokenRequestDto(string UserId, string Provider, string Token);
}