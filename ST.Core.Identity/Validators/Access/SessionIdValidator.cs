using Microsoft.Extensions.DependencyInjection;
using ST.Core.Registration.Attributes;
using ST.Core.Validators;
using ST.Core.Validators.Results;
using ST.Core.Validators.Results.Interfaces;

namespace ST.Core.Identity.Validators.Access
{
    /// <summary>
    /// Validates that a session ID is present and non-empty.
    /// </summary>
    [AutoRegister(ServiceLifetime.Singleton)]
    public class SessionIdValidator : IValidator<string>
    {
        /// <summary>
        /// Validates the specified session ID.
        /// </summary>
        /// <param name="input">The session ID to validate.</param>
        /// <returns>
        /// An <see cref="IValidationResult"/> indicating success if the ID is valid,
        /// or failure if it is missing or empty.
        /// </returns>
        public IValidationResult Validate(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return ValidationResultFactory.Failure(
                    message: "Session ID is required.",
                    code: "MissingSessionId",
                    field: nameof(input));
            }

            return ValidationResultFactory.Success();
        }
    }
}
