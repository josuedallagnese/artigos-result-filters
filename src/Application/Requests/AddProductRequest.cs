using ResultFilters.Core;
using FluentValidation;
using MediatR;

namespace ResultFilters.Application.Requests
{
    public class AddProductRequest : IRequest<ApiResponse<string>>
    {
        public string Code { get; set; }
        public double Value { get; set; }
        public int Quantity { get; set; }
    }

    public class AddProductRequestValidator : AbstractValidator<AddProductRequest>
    {
        public AddProductRequestValidator()
        {
            RuleFor(r => r.Code).NotNull().MaximumLength(100);
            RuleFor(r => r.Value).GreaterThan(0);
            RuleFor(r => r.Quantity).GreaterThan(0);
        }
    }
}
