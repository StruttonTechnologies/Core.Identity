using Microsoft.Extensions.DependencyInjection;
using ST.Core.Registration.Attributes;
using ST.Core.Validators;
using ST.Core.Validators.Results;
using ST.Core.Validators.Results.Interfaces;
using System.Text.RegularExpressions;

namespace ST.Core.Identity.Validators.Security
{
    /// <summary>
    /// Validates that a password matches a specified regular expression pattern.
    /// </summary>
    [AutoRegister(ServiceLifetime.Singleton)]
    public class RegexPasswordValidator : IValidator<string>
    {
        private readonly Regex _regex;
        private readonly string _patternDescription;
        private readonly string _fieldName;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegexPasswordValidator"/> class.
        /// </summary>
        /// <param name="pattern">The regex pattern to enforce.</param>
        /// <param name="patternDescription">A human-readable description of the pattern.</param>
        /// <param name="fieldName">Optional field name for diagnostics.</param>
        public RegexPasswordValidator(string pattern, string patternDescription, string fieldName = "Password")
        {
            _regex = new Regex(pattern, RegexOptions.Compiled);
            _patternDescription = patternDescription;
            _fieldName = fieldName;
        }

        /// <summary>
        /// Validates that the password matches the configured regex pattern.
        /// </summary>
        /// <param name="input">The password to validate.</param>
        /// <returns>
        /// An <see cref="IValidationResult"/> indicating success if the pattern matches,
        /// or failure with a descriptive message.
        /// </returns>
        public IValidationResult Validate(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return ValidationResultFactory.Failure(
                    message: "Password is required.",
                    code: "Required",
                    field: _fieldName);
            }

            if (!_regex.IsMatch(input))
            {
                return ValidationResultFactory.Failure(
                    message: $"Password must match pattern: {_patternDescription}.",
                    code: "PatternMismatch",
                    field: _fieldName);
            }

            return ValidationResultFactory.Success();
        }
    }

}
