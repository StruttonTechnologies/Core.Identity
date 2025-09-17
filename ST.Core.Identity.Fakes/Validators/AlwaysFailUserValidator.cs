using Microsoft.AspNetCore.Identity;
using ST.Core.Identity.Fakes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Fakes.Validators
{
    /// <summary>
    /// A user validator that always fails validation for <see cref="TestAppIdentityUser"/>.
    /// </summary>
    public class AlwaysFailUserValidator : IUserValidator<TestAppIdentityUser>
    {
        /// <summary>
        /// Validates the specified user and always returns a failed <see cref="IdentityResult"/>.
        /// </summary>
        /// <param name="manager">The <see cref="UserManager{TestAppIdentityUser}"/> instance.</param>
        /// <param name="user">The <see cref="TestAppIdentityUser"/> to validate.</param>
        /// <returns>
        /// A <see cref="Task{IdentityResult}"/> that represents the asynchronous operation.
        /// The task result contains a failed <see cref="IdentityResult"/> with an invalid email error.
        /// </returns>
        public Task<IdentityResult> ValidateAsync(UserManager<TestAppIdentityUser> manager, TestAppIdentityUser user)
        {
            return Task.FromResult(IdentityResult.Failed(new IdentityError
            {
                Code = "InvalidEmail",
                Description = "Email format is invalid."
            }));
        }
    }
}
