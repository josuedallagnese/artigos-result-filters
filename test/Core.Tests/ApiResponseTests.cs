using Xunit;

namespace ResultFilters.Core.Tests
{
    public class ApiResponseTests
    {
        [Fact]
        public void ApiResponse_ShouldNotInclude_DuplicatedErrors()
        {
            var response = new ApiResponse();

            response.Add("message");
            response.Add("message");

            Assert.True(response.HasErrors);
            Assert.Single(response.Errors);
        }

        [Fact]
        public void ApiResponse_ShouldRemove_Error()
        {
            var response = new ApiResponse();

            response.Add("error");

            Assert.Single(response.Errors);

            response.Remove("error");

            Assert.Empty(response.Errors);
        }

        [Fact]
        public void ApiResponse_Should_Be_Equals()
        {
            var responseA = new ApiResponse();
            var responseB = new ApiResponse();

            responseA.Add("One Error");
            responseA.Add("Two Errors");
            responseA.Add("Three Errors");

            responseB.Add("One Error");
            responseB.Add("Two Errors");
            responseB.Add("Three Errors");

            Assert.Equal(responseA, responseB);
            Assert.Equal(responseB, responseA);
        }

        [Fact]
        public void ApiResponse_Should_Be_Equals_IfEmpty()
        {
            var responseA = new ApiResponse();
            var responseB = new ApiResponse();

            Assert.Equal(responseA, responseB);
            Assert.Equal(responseB, responseA);
        }

        [Fact]
        public void ApiResponse_Should_Be_Equals_IfNull()
        {
            ApiResponse responseA = null;
            ApiResponse responseB = null;

            Assert.Equal(responseB, responseA);

            responseA = new ApiResponse();
            responseB = null;

            Assert.NotEqual(responseB, responseA);

            responseA = null;
            responseB = new ApiResponse();

            Assert.NotEqual(responseB, responseA);
        }

        [Fact]
        public void ApiResponse_Should_Be_NotEquals_IfNull()
        {
            ApiResponse responseA = new ApiResponse();
            ApiResponse responseB = null;

            Assert.NotEqual(responseB, responseA);

            responseA = null;
            responseB = new ApiResponse();

            Assert.NotEqual(responseB, responseA);
        }

        [Fact]
        public void ApiResponse_Should_Be_NotEquals_OnError_Qtd()
        {
            var responseA = new ApiResponse();
            var responseB = new ApiResponse();

            responseA.Add("One Error");
            responseB.Add("One Error");
            responseB.Add("Two Errors");

            Assert.NotEqual(responseA, responseB);
        }
    }
}
