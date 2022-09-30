using System.Threading;
using System.Threading.Tasks;
using ResultFilters.Core.Exceptions;
using MediatR;
using System.Collections.Generic;

namespace ResultFilters.Core.MediatR
{
    public class ValidationMediator : IMediator
    {
        private readonly IMediator _innerMediator;

        public ValidationMediator(ServiceFactory serviceFactory)
        {
            _innerMediator = new Mediator(serviceFactory);
        }

        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            if (request == null)
                throw new NullRequestParameterException();

            return _innerMediator.Send(request, cancellationToken);
        }

        public Task<object> Send(object request, CancellationToken cancellationToken = default)
        {
            if (request == null)
                throw new NullRequestParameterException();

            return _innerMediator.Send(request, cancellationToken);
        }

        public Task Publish(object notification, CancellationToken cancellationToken = default)
            => _innerMediator.Publish(notification, cancellationToken);

        public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification
            => _innerMediator.Publish(notification, cancellationToken);

        public IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamRequest<TResponse> request, CancellationToken cancellationToken = default)
            => _innerMediator.CreateStream(request, cancellationToken);

        public IAsyncEnumerable<object> CreateStream(object request, CancellationToken cancellationToken = default)
            => _innerMediator.CreateStream(request, cancellationToken);
    }
}
