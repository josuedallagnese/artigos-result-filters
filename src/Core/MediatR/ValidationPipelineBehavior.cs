using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace ResultFilters.Core.MediatR
{
    public class ValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
       where TRequest : IRequest<TResponse> where TResponse : ApiResponse, new()
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var failures = GetFailures(request);

            if (failures.Any())
                return Errors(failures);

            return next();
        }

        private IEnumerable<ValidationFailure> GetFailures(TRequest request)
        {
            foreach (var validator in _validators)
            {
                var validationResult = validator.Validate(request);

                foreach (var error in validationResult.Errors)
                    yield return error;
            }
        }

        private static Task<TResponse> Errors(IEnumerable<ValidationFailure> failures)
        {
            var response = new TResponse();

            foreach (var failure in failures)
                response.Add(failure.ErrorMessage);

            return Task.FromResult(response);
        }
    }
}
