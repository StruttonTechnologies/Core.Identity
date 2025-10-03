namespace ST.Core.Identity.Dtos.Notifications
{
    public record NotificationRequestDto(string Destination, string Subject, string Body);

    public record NotificationResponseDto(bool Success, string Message);
}
