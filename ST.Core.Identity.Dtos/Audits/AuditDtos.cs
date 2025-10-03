namespace ST.Core.Identity.Dtos.Audits
{
    public record AuditEventDto(DateTime Timestamp, string EventType, string UserName, string Details);
}
