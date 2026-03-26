namespace StruttonTechnologies.Core.Identity.Dtos.Authorization
{
    public record UserRolesDto(string UserId, IList<string> Roles);
}
