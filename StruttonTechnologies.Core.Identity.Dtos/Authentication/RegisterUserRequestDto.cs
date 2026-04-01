using System.Diagnostics.CodeAnalysis;

namespace StruttonTechnologies.Core.Identity.Dtos.Authentication
{
    [ExcludeFromCodeCoverage]

    public record RegisterUserRequestDto(
        string Email,
        string Password,
        string DisplayName);
}
