using System.Diagnostics.CodeAnalysis;

namespace ST.Core.Identity.Dtos.Authentication.User
{
    [ExcludeFromCodeCoverage]
    public record CreateUserRequestDto(string UserName, string Email, string Password);
}