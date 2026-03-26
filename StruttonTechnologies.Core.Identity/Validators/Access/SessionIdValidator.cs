namespace StruttonTechnologies.Core.Identity.Validators.Access
{
    /// <summary>
    /// Validates that a session ID is present and non-empty.
    /// </summary>
    public class SessionIdValidator : IValidator<string>
    {
        /// <summary>
        /// Validates the specified session ID.
        /// </summary>
        /// <param name="input">The session ID to validate.</param>
        /// <returns>
        /// An <see cref="ValidationResult"/> indicating success if the ID is valid,
        /// or failure if it is missing or empty.
        /// </returns>
        public ValidationResult Validate(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return ValidationResult.Failure("Session ID is required.", "Required", "SessionId");
            }

            if (!Guid.TryParse(input, out Guid parsed) || parsed == Guid.Empty)
            {
                return ValidationResult.Failure("Invalid Session ID format.", "InvalidSessionId", "SessionId");
            }

            return ValidationResult.Success();
        }
    }
}
