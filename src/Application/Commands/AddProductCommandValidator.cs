using FluentValidation;

namespace ResultFilters.Application.Commands
{
    public class AddProductCommandValidator : AbstractValidator<AddProductCommand>
    {
        public AddProductCommandValidator()
        {
            RuleFor(r => r.Code).NotNull().MaximumLength(100);
            RuleFor(r => r.Value).GreaterThan(0);
            RuleFor(r => r.Quantity).GreaterThan(0);
        }
    }
}
