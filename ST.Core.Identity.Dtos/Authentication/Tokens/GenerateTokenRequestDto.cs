using System.Diagnostics.CodeAnalysis;

namespace ST.Core.Identity.Dtos.Authentication.Tokens
{
    [ExcludeFromCodeCoverage]
    public record GenerateTokenRequestDto(string UserId, string TokenProvider);
}