using Microsoft.Extensions.DependencyInjection;
using ST.Core.Identity.Data;
using ST.Core.Registration.Attributes;
using ST.Core.Validators;
using ST.Core.Validators.Results;
using ST.Core.Validators.Results.Interfaces;

namespace ST.Core.Identity.Validators.Access
{
    /// <summary>
    /// Validates that a role is part of an allowed set of roles.
    /// </summary>
    [AutoRegister(ServiceLifetime.Singleton)]
    public class RoleValidator : IValidator<string>
    {
        /// <summary>
        /// Validates that the input role is allowed.
        /// </summary>
        /// <param name="input">The role to validate.</param>
        /// <returns>
        /// An <see cref="IValidationResult"/> indicating success if the role is valid,
        /// or failure if it is not recognized.
        /// </returns>
        public IValidationResult Validate(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return ValidationResultFactory.Failure(
                    message: "Role is required.",
                    code: "Required",
                    field: nameof(input));
            }

            if (!IdentitySeed.AllowedRoles.Contains(input))
            {
                return ValidationResultFactory.Failure(
                    message: $"Role '{input}' is not recognized or authorized.",
                    code: "InvalidRole",
                    field: nameof(input));
            }

            return ValidationResultFactory.Success();
        }
    }
}