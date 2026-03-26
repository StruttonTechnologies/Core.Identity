using StruttonTechnologies.Core.Identity.Data;

namespace StruttonTechnologies.Core.Identity.Validators.Access
{
    /// <summary>
    /// Validates that a role is part of an allowed set of roles.
    /// </summary>
    public class RoleValidator : IValidator<string>
    {
        /// <summary>
        /// Validates that the input role is allowed.
        /// </summary>
        /// <param name="input">The role to validate.</param>
        /// <returns>
        /// An <see cref="ValidationResult"/> indicating success if the role is valid,
        /// or failure if it is not recognized.
        /// </returns>
        public ValidationResult Validate(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return ValidationResult.Failure(
                    message: "Role is required.",
                    code: "Required",
                    field: nameof(input));
            }

            if (!KnownRoles.All.Contains(input))
            {
                return ValidationResult.Failure(
                    message: $"Role '{input}' is not recognized or authorized.",
                    code: "InvalidRole",
                    field: nameof(input));
            }

            return ValidationResult.Success();
        }
    }
}
