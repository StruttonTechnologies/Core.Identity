using Microsoft.Extensions.DependencyInjection;
using ST.Core.Registration.Attributes;
using ST.Core.Validators;
using ST.Core.Validators.Results;
using ST.Core.Validators.Results.Interfaces;

namespace ST.Core.Identity.Validators.Security
{
    /// <summary>
    /// Validates that a password is not present in a predefined blacklist.
    /// </summary>
    [AutoRegister(ServiceLifetime.Singleton)]
    public class BlacklistPasswordValidator : IValidator<string>
    {
        private readonly HashSet<string> _blacklistedPasswords;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlacklistPasswordValidator"/> class.
        /// </summary>
        /// <param name="blacklistedPasswords">A collection of passwords to block.</param>
        public BlacklistPasswordValidator(IEnumerable<string> blacklistedPasswords)
        {
            _blacklistedPasswords = new HashSet<string>(blacklistedPasswords);
        }

        /// <summary>
        /// Validates that the input password is not blacklisted.
        /// </summary>
        /// <param name="input">The password to validate.</param>
        /// <returns>
        /// An <see cref="IValidationResult"/> indicating success if the password is allowed,
        /// or failure if it is blacklisted.
        /// </returns>
        public IValidationResult Validate(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return ValidationResultFactory.Failure(
                    message: "Password is required.",
                    code: "Required",
                    field: nameof(input));
            }

            if (_blacklistedPasswords.Contains(input))
            {
                return ValidationResultFactory.Failure(
                    message: "Password is too common or insecure.",
                    code: "BlacklistedPassword",
                    field: nameof(input));
            }

            return ValidationResultFactory.Success();
        }
    }


}
