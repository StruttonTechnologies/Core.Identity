using Microsoft.Extensions.DependencyInjection;
using ST.Core.Registration.Attributes;
using ST.Core.Validators.Results.Interfaces;
using ST.Core.Validators.Results.Models;

namespace ST.Core.Identity.Validators.Access
{
    /// <summary>
    /// Validates that a tenant ID is present, well-formed, and not Guid.Empty.
    /// </summary>
    [AutoRegister(ServiceLifetime.Singleton)]
    public class TenantIdValidator : IValidator<string>
    {
        /// <summary>
        /// Validates the specified tenant ID string.
        /// </summary>
        /// <param name="input">The tenant ID to validate.</param>
        /// <returns>
        /// An <see cref="IValidationResult"/> indicating success if the ID is valid,
        /// or failure if it is missing, malformed, or represents Guid.Empty.
        /// </returns>
        public ValidationResult Validate(string input)
        {
            const string fieldName = "TenantId";

            if (string.IsNullOrWhiteSpace(input))
            {
                return ValidationResult.Failure(
                    message: "Tenant ID is required.",
                    code: "MissingTenantId",
                    field: fieldName);
            }

            if (!Guid.TryParse(input, out var parsed) || parsed == Guid.Empty)
            {
                return ValidationResult.Failure(
                    message: "Tenant ID must be a valid non-empty GUID.",
                    code: "InvalidTenantId",
                    field: fieldName);
            }

            return ValidationResult.Success();
        }
    }
}