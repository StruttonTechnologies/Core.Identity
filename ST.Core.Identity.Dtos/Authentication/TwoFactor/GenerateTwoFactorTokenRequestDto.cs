using System.Diagnostics.CodeAnalysis;

namespace ST.Core.Identity.Dtos.Authentication.TwoFactor
{
    [ExcludeFromCodeCoverage]
    public record GenerateTwoFactorTokenRequestDto(string UserId, string Provider);
}