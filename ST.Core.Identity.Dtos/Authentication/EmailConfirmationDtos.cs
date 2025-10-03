namespace ST.Core.Identity.Dtos.Authentication
{
    public record EmailConfirmationRequestDto(Guid UserId, string Email);

    public record EmailConfirmationResponseDto(bool Success, string Message);
}
