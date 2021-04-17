using System;

namespace ResultFilters.Core
{
    public class ApiResponse<TResult> : ApiResponse
    {
        public TResult Result { get; set; }

        public ApiResponse()
        {
        }

        public ApiResponse(TResult result) => Result = result;

        public TReturn Add<TReturn>(ApiResponse<TReturn> response)
        {
            if (response is null)
                throw new ArgumentNullException(nameof(response));

            foreach (var error in response.Errors)
            {
                Add(error);
            }

            return response.Result;
        }
    }
}
