using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Fakes.Validators.Interfaces
{
    /// <summary>
    /// Defines a contract for validating phone numbers for a given user type.
    /// </summary>
    /// <typeparam name="TUser">The user type.</typeparam>
    public interface IPhoneNumberValidator<TUser>
    {
        /// <summary>
        /// Validates the specified phone number for the given user.
        /// </summary>
        /// <param name="user">The user whose phone number is being validated.</param>
        /// <param name="phoneNumber">The phone number to validate.</param>
        /// <returns>An <see cref="IdentityResult"/> indicating success or failure.</returns>
        Task<IdentityResult> ValidateAsync(TUser user, string phoneNumber);
    }
}
