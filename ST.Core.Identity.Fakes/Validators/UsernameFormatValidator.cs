using Microsoft.AspNetCore.Identity;
using ST.Core.Identity.Fakes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ST.Core.Identity.Fakes.Validators
{
    /// <summary>
    /// Validates that the username contains only allowed characters and meets length requirements.
    /// </summary>
    public class UserNameFormatValidator : IUserValidator<TestAppIdentityUser>
    {
        private readonly Regex _pattern;
        private readonly int _minLength;
        private readonly int _maxLength;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserNameFormatValidator"/> class.
        /// </summary>
        /// <param name="pattern">A regular expression pattern that the username must match.</param>
        /// <param name="minLength">Minimum allowed length for the username.</param>
        /// <param name="maxLength">Maximum allowed length for the username.</param>
        public UserNameFormatValidator(string pattern = @"^[a-zA-Z0-9_.-]+$", int minLength = 3, int maxLength = 20)
        {
            _pattern = new Regex(pattern);
            _minLength = minLength;
            _maxLength = maxLength;
        }

        /// <summary>
        /// Validates the specified user's username against format and length rules.
        /// </summary>
        /// <param name="manager">The user manager.</param>
        /// <param name="user">The user to validate.</param>
        /// <returns>
        /// An <see cref="IdentityResult"/> indicating success or failure.
        /// </returns>
        public Task<IdentityResult> ValidateAsync(UserManager<TestAppIdentityUser> manager, TestAppIdentityUser user)
        {
            var errors = new List<IdentityError>();
            var username = user.UserName ?? "";

            if (username.Length < _minLength)
            {
                errors.Add(new IdentityError
                {
                    Code = "UserNameTooShort",
                    Description = $"Username must be at least {_minLength} characters long."
                });
            }

            if (username.Length > _maxLength)
            {
                errors.Add(new IdentityError
                {
                    Code = "UserNameTooLong",
                    Description = $"Username must not exceed {_maxLength} characters."
                });
            }

            if (!_pattern.IsMatch(username))
            {
                errors.Add(new IdentityError
                {
                    Code = "UserNameInvalidCharacters",
                    Description = "Username contains invalid characters. Only letters, digits, '.', '_', and '-' are allowed."
                });
            }

            return Task.FromResult(errors.Any() ? IdentityResult.Failed(errors.ToArray()) : IdentityResult.Success);
        }
    }
}
