namespace StruttonTechnologies.Core.Identity.Validators.Identity
{
    /// <summary>
    /// Validates that a user's email has been confirmed.
    /// </summary>
    public class EmailConfirmedValidator : IValidator<bool>
    {
        /// <summary>
        /// Validates the email confirmation status.
        /// </summary>
        /// <param name="input">The email confirmation flag.</param>
        /// <returns>
        /// An <see cref="ValidationResult"/> indicating success if confirmed,
        /// or failure if not confirmed.
        /// </returns>
        public ValidationResult Validate(bool input)
        {
            if (!input)
            {
                return ValidationResult.Failure(
                    message: "Email address has not been confirmed.",
                    code: "EmailNotConfirmed",
                    field: nameof(input));
            }

            return ValidationResult.Success();
        }
    }
}
