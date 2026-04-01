using System.Diagnostics.CodeAnalysis;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace StruttonTechnologies.Core.Identity.Coordinator.Contracts.Users.Queries
{
    /// <summary>
    /// Query to retrieve all external login providers linked to a user.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed record GetUserLoginsQuery(string UserId) : IRequest<IList<UserLoginInfo>>;
}
