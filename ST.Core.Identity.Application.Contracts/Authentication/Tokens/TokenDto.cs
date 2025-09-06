namespace ST.Core.Identity.Application.Contracts.Authentication.Tokens
{
    public record TokenDto(string UserId, string? TokenProvider = null, string? Token = null);
}