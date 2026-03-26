using Microsoft.AspNetCore.Identity;

namespace StruttonTechnologies.Core.Identity.Fakes.Validators
{
    /// <summary>
    /// A test validator that always fails when validating an IdentityUser.
    /// Implements IUserValidator to simulate failure in ASP.NET Identity pipelines.
    /// </summary>
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
