namespace ST.Core.Identity.Dtos.Authorization.Claims
{
    public record HasClaimRequestDto(string UserId, string Type, string Value);
}