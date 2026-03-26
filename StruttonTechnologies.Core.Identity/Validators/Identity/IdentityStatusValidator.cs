namespace StruttonTechnologies.Core.Identity.Validators.Identity
{
    public enum IdentityStatus
    {
        Active,        // Fully authenticated and authorized
        Pending,       // Awaiting verification or approval
        Suspended,     // Temporarily disabled (e.g., due to policy violation)
        Deactivated,   // Permanently disabled or removed
        Locked,        // Blocked due to failed login attempts or security flags
        Invited,        // User has been invited but not yet onboarded
        Local
    }

    /// <summary>
    /// Validates that an identity status is active and authorized.
    /// </summary>
    public class IdentityStatusValidator : IValidator<IdentityStatus>
    {
        /// <summary>
        /// Validates the specified identity status.
        /// </summary>
        /// <param name="input">The identity status to validate.</param>
        /// <returns>
        /// An <see cref="ValidationResult"/> indicating success if the status is valid,
        /// or failure if the identity is inactive or unauthorized.
        /// </returns>
        public ValidationResult Validate(IdentityStatus input)
        {
            if (input != IdentityStatus.Active)
            {
                return ValidationResult.Failure(
                    message: $"Identity status '{input}' is not authorized.",
                    code: "InvalidIdentityStatus",
                    field: nameof(input));
            }

            return ValidationResult.Success();
        }
    }
}
