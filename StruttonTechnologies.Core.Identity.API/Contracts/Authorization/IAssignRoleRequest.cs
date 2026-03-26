namespace StruttonTechnologies.Core.Identity.API.Contracts.Authorization
{
    public interface IAssignRoleRequest
    {
        string UserId { get; }
        string RoleName { get; }
    }
}
