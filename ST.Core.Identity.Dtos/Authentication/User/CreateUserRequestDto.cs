namespace ST.Core.Identity.Dtos.Authentication.User
{
    public record CreateUserRequestDto(string UserName, string Email, string Password);
}