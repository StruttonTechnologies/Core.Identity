using System.Diagnostics.CodeAnalysis;

namespace ST.Core.Identity.Dtos.Authentication.Tokens
{
    [ExcludeFromCodeCoverage]
    public record ConfirmEmailRequestDto(string UserId, string Token);
}