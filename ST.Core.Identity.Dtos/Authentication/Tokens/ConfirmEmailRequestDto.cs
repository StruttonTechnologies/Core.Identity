namespace ST.Core.Identity.Dtos.Authentication.Tokens
{
    public record ConfirmEmailRequestDto(string UserId, string Token);
}