namespace StruttonTechnologies.Core.Identity.API.Contracts.Authorization
{
    public interface IRemoveRoleRequest
    {
        string UserId { get; }
        string RoleName { get; }
    }
}
