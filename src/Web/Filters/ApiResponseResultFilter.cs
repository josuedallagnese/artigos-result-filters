using System.Threading.Tasks;
using ResultFilters.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;

namespace ResultFilters.Web.Filters
{
    public class ResponseResultFilter : IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (context.Result is ObjectResult objectResult &&
                objectResult.Value is ApiResponse response)
            {
                if (response.HasErrors)
                {
                    var outResponse = new ApiResponse()
                        .Add(response);

                    objectResult.StatusCode = StatusCodes.Status400BadRequest;
                    objectResult.Value = outResponse;
                }
            }

            await next();
        }
    }
}
