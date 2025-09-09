using System.Diagnostics.CodeAnalysis;

namespace ST.Core.Identity.Dtos.Authentication.TwoFactor
{
    [ExcludeFromCodeCoverage]
    public record TwoFactorStatusResponseDto(string UserId, bool Enabled, IEnumerable<string> Providers);
}