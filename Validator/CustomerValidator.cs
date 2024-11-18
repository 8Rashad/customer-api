using FluentValidation;
using TaskApi.Entity;
using TaskApi.Request;

namespace TaskApi.Validator
{
    public class CustomerValidator : AbstractValidator<CustomerRequest>
    {
        public CustomerValidator() 
        {
            RuleFor(c => c.FirstName).NotEmpty().WithMessage("FirstName should not be empty");
            RuleFor(c => c.LastName).NotEmpty().WithMessage("LastName should not be empty");
            RuleFor(c => c.Email).EmailAddress().WithMessage("Incorrect email address format");
            RuleFor(c => c.Balance).GreaterThan(0).WithMessage("Balance must be greater than 0.").NotNull().WithMessage("Balance cannot be null.");
        }
    }
}
