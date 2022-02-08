using Application.Features.CustomerFeatures.Commands.RegisterCustomer;
using FluentValidation;

namespace WebApi.Customers.Validation
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class RegisterCustomerRequestValidator : AbstractValidator<RegisterCustomerRequest>
    {
        public RegisterCustomerRequestValidator()
        {
            RuleFor(request => request.FirstName).NotEmpty().MaximumLength(50);
            RuleFor(request => request.LastName).NotEmpty().MaximumLength(50);
            RuleFor(request => request.Email).EmailAddress();
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
