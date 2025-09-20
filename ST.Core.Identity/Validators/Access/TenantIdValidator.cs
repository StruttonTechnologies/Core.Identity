using Microsoft.Extensions.DependencyInjection;
using ST.Core.Registration.Attributes;
using ST.Core.Validators;
using ST.Core.Validators.Results;
using ST.Core.Validators.Results.Interfaces;

namespace ST.Core.Identity.Validators.Access
{
    /// <summary>
    /// Validates that a tenant ID is present and non-empty.
    /// </summary>
    [AutoRegister(ServiceLifetime.Singleton)]
    public class TenantIdValidator : IValidator<string>
    {
        /// <summary>
        /// Validates the specified tenant ID.
        /// </summary>
        /// <param name="input">The tenant ID to validate.</param>
        /// <returns>
        /// An <see cref="IValidationResult"/> indicating success if the ID is valid,
        /// or failure if it is missing or empty.
        /// </returns>
        public IValidationResult Validate(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return ValidationResultFactory.Failure(
                    message: "Tenant ID is required.",
                    code: "MissingTenantId",
                    field: nameof(input));
            }

            return ValidationResultFactory.Success();
        }
    }
}
