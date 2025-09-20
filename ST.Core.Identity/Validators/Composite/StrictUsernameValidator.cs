using Microsoft.Extensions.DependencyInjection;
using ST.Core.Registration.Attributes;
using ST.Core.Validators.Results;
using ST.Core.Validators.Results.Interfaces;

namespace ST.Core.Validators.Composite
{
    /// <summary>
    /// Validates that a username is present, formatted correctly, and not reserved.
    /// </summary>
    [AutoRegister(ServiceLifetime.Singleton)]
    public class StrictUsernameValidator : IValidator<string>
    {
        private readonly IValidator<string> _requiredValidator;
        private readonly IValidator<string> _formatValidator;
        private readonly IValidator<string> _reservedValidator;

        public StrictUsernameValidator(
            IValidator<string> requiredValidator,
            IValidator<string> formatValidator,
            IValidator<string> reservedValidator)
        {
            _requiredValidator = requiredValidator;
            _formatValidator = formatValidator;
            _reservedValidator = reservedValidator;
        }

        public IValidationResult Validate(string input)
        {
            var requiredResult = _requiredValidator.Validate(input);
            if (!requiredResult.IsSuccess) return requiredResult;

            var formatResult = _formatValidator.Validate(input);
            if (!formatResult.IsSuccess) return formatResult;

            var reservedResult = _reservedValidator.Validate(input);
            if (!reservedResult.IsSuccess) return reservedResult;

            return ValidationResultFactory.Success();
        }
    }
}
