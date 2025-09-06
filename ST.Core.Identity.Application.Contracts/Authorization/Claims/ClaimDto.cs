namespace ST.Core.Identity.Application.Contracts.Authorization.Claims
{
    public record ClaimDto(string UserId, string Type, string Value);
}