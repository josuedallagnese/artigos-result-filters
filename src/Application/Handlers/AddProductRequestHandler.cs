using System;
using System.Threading;
using System.Threading.Tasks;
using ResultFilters.Application.Commands;
using ResultFilters.Core;
using MediatR;

namespace ResultFilters.Application.Handlers
{
    public class AddProductRequestHandler : IRequestHandler<AddProductCommand, ApiResponse<string>>
    {
        public async Task<ApiResponse<string>> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var response = new ApiResponse<string>();

            // ...
            await Task.Delay(1);

            response.Result = Guid.NewGuid().ToString();

            return response;
        }
    }
}
