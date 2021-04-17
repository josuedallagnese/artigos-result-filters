using ResultFilters.Core.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Builder
{
    public static class GlobalExceptionHandler
    {
        public static void UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    var feature = context.Features.Get<IExceptionHandlerPathFeature>();
                    var exception = feature.Error;

                    var result = NullRequestParameterException.ExceptionApiResponse;

                    if (exception is NullRequestParameterException)
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;

                        result = NullRequestParameterException.NullRequestApiResponse;
                    }

                    context.Response.ContentType = "application/json";

                    await context.Response.WriteAsync(result);
                });
            });
        }
    }
}
