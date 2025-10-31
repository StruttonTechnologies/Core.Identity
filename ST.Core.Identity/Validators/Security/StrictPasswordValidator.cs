using Microsoft.Extensions.DependencyInjection;
using ST.Core.Registration.Attributes;
using ST.Core.Validators.Composition;
using ST.Core.Validators.Results.Interfaces;
using ST.Core.Validators.Results.Models;

namespace ST.Core.Identity.Validators.Security
{
    /// <summary>
    /// Validates that a password meets all strict security requirements.
    /// Combines multiple validators: format, blacklist, whitespace, and regex-based rules.
    /// </summary>
    [AutoRegister(ServiceLifetime.Singleton)]
    public class StrictPasswordValidator : IValidator<string>
    {
        private readonly CompositeValidator<string> _composite;

        /// <summary>
        /// Initializes a new instance of the <see cref="StrictPasswordValidator"/> class.
        /// </summary>
        public StrictPasswordValidator()
        {
            _composite = new CompositeValidator<string>(new IValidator<string>[]
            {
                //new NoWhitespacePasswordValidator(),
                new BlacklistPasswordValidator(new[] { "password", "123456", "qwerty", "letmein" }),
                new RegexPasswordValidator(@"^(?=.*[A-Z])", "at least one uppercase letter"),
                new RegexPasswordValidator(@"^(?=.*[a-z])", "at least one lowercase letter"),
                new RegexPasswordValidator(@"^(?=.*\d)", "at least one digit"),
                new RegexPasswordValidator(@"^(?=.*[!@#$%^&*])", "at least one special character"),
                new RegexPasswordValidator(@".{8,}", "minimum length of 8 characters")
            });
        }

        /// <summary>
        /// Validates the password using all strict rules.
        /// </summary>
        /// <param name="input">The password to validate.</param>
        /// <returns>
        /// An <see cref="IValidationResult"/> indicating success if all rules pass,
        /// or the first failure encountered.
        /// </returns>
        public ValidationResult Validate(string input) => _composite.Validate(input);
    }


}
