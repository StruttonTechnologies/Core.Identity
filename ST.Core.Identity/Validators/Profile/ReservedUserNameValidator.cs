using Microsoft.Extensions.DependencyInjection;
using ST.Core.Registration.Attributes;
using ST.Core.Validators;
using ST.Core.Validators.Results;
using ST.Core.Validators.Results.Interfaces;

namespace ST.Core.Identity.Validators.Profile
{
    /// <summary>
    /// Validates that a username is not part of a reserved list.
    /// </summary>
    [AutoRegister(ServiceLifetime.Singleton)]
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
        /// An <see cref="IValidationResult"/> indicating success if the username is allowed,
        /// or failure if it is reserved.
        /// </returns>
        public IValidationResult Validate(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return ValidationResultFactory.Failure(
                    message: "Username is required.",
                    code: "Required",
                    field: nameof(input));
            }

            if (_reservedUsernames.Contains(input))
            {
                return ValidationResultFactory.Failure(
                    message: "This username is reserved and cannot be used.",
                    code: "ReservedUsername",
                    field: nameof(input));
            }

            return ValidationResultFactory.Success();
        }
    }


}

