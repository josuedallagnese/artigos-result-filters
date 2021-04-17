using ResultFilters.Core;
using MediatR;

namespace ResultFilters.Application.Commands
{
    public class AddProductCommand : IRequest<ApiResponse<string>>
    {
        public string Code { get; set; }
        public double Value { get; set; }
        public int Quantity { get; set; }
    }
}
