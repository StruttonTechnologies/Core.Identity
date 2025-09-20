using Microsoft.Extensions.DependencyInjection;
using ST.Core.Identity.Validators.Identity;
using ST.Core.Registration.Attributes;
using ST.Core.Validators.Results;
using ST.Core.Validators.Results.Interfaces;

namespace ST.Core.Identity.Validators.Composite
{
    /// <summary>
    /// Validates that a user's identity is ready for access: status is active and email is confirmed.
    /// </summary>
    [AutoRegister(ServiceLifetime.Singleton)]
    public class IdentityReadinessValidator : IValidator<IdentityContext>
    {
        private readonly IValidator<IdentityStatus> _statusValidator;
        private readonly IValidator<bool> _emailConfirmedValidator;

        public IdentityReadinessValidator(
            IValidator<IdentityStatus> statusValidator,
            IValidator<bool> emailConfirmedValidator)
        {
            _statusValidator = statusValidator;
            _emailConfirmedValidator = emailConfirmedValidator;
        }

        public IValidationResult Validate(IdentityContext input)
        {
            var statusResult = _statusValidator.Validate(input.Status);
            if (!statusResult.IsSuccess) return statusResult;

            var emailResult = _emailConfirmedValidator.Validate(input.EmailConfirmed);
            if (!emailResult.IsSuccess) return emailResult;

            return ValidationResultFactory.Success();
        }
    }

    public record IdentityContext(IdentityStatus Status, bool EmailConfirmed);
}