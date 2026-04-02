using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore.Identity;

namespace StruttonTechnologies.Core.Identity.Fakes.Validators
{
    /// <summary>
    /// A test validator that always fails when validating an IdentityUser.
    /// Implements to simulate failure in ASP.NET Identity pipelines.
    /// </summary>
    /// <typeparam name="TUser">The user type being validated.</typeparam>
    [ExcludeFromCodeCoverage]
    public class AlwaysFailUserValidator<TUser> : IUserValidator<TUser>
        where TUser : IdentityUser<Guid>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user)
        {
            IdentityError error = new IdentityError
            {
                Code = "InvalidEmail",
                Description = "Email format is invalid."
            };

            return Task.FromResult(IdentityResult.Failed(error));
        }
    }
}
