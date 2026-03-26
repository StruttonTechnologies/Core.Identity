namespace StruttonTechnologies.Core.Identity.Validators.Profile
{
    /// <summary>
    /// Validates that a username is not part of a reserved list.
    /// </summary>
    public class ReservedUserNameValidator : IValidator<string>
    {
        private readonly HashSet<string> _reservedUsernames;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReservedUserNameValidator"/> class.
        /// </summary>
        /// <param name="reservedUsernames">A collection of reserved usernames to block.</param>
        public ReservedUserNameValidator(IEnumerable<string> reservedUsernames)
        {
            _reservedUsernames = new HashSet<string>(reservedUsernames, System.StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Validates that the input username is not reserved.
        /// </summary>
        /// <param name="input">The username to validate.</param>
        /// <returns>
        /// An <see cref="ValidationResult"/> indicating success if the username is allowed,
        /// or failure if it is reserved.
        /// </returns>
        public ValidationResult Validate(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return ValidationResult.Failure(
                    message: "Username is required.",
                    code: "Required",
                    field: nameof(input));
            }

            if (_reservedUsernames.Contains(input))
            {
                return ValidationResult.Failure(
                    message: "This username is reserved and cannot be used.",
                    code: "ReservedUsername",
                    field: nameof(input));
            }

            return ValidationResult.Success();
        }
    }
}
