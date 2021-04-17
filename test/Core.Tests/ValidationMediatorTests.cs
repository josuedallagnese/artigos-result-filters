using System;
using System.Threading;
using System.Threading.Tasks;
using ResultFilters.Core.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ResultFilters.Core
{
    public class ValidationMediatorTests
    {
        private readonly IServiceProvider _provider;
        private readonly IMediator _mediator;
        private readonly Counter _counter;

        public ValidationMediatorTests()
        {
            _provider = new ServiceCollection()
                .AddCoreMediator(typeof(ValidationMediatorTests).Assembly)
                .AddSingleton<Counter>()
                .BuildServiceProvider();

            _mediator = _provider.GetRequiredService<IMediator>();
            _counter = _provider.GetRequiredService<Counter>();
        }

        [Fact]
        public async Task Mediator_Command_Simple()
        {
            await _mediator.Send(new SimpleCommand());

            Assert.Equal(1, _counter.Count);
        }

        [Fact]
        public async Task Mediator_Command_WithResponse()
        {
            var response = await _mediator.Send(new ResponseCommand());

            Assert.True(response.HasErrors);
        }

        [Fact]
        public async Task Mediator_Command_WithGenericResponse()
        {
            var response = await _mediator.Send(new GenericResponseCommand());

            Assert.Equal("user", response.Result.Name);
            Assert.True(response.HasErrors);
        }

        [Fact]
        public async Task Append_Should_Return_Result()
        {
            var response = new ApiResponse<User>();

            var user = response.Add(await _mediator.Send(new GenericResponseCommand()));

            Assert.Equal("user", user.Name);
            Assert.True(response.HasErrors);
        }

        [Fact]
        public async Task Mediator_Notification()
        {
            await _mediator.Publish(new Ping());

            Assert.Equal(2, _counter.Count);
        }

        [Fact]
        public async Task Mediator_SendNullCommand_ShouldThrowsException()
        {
            ValidatableCommand command = null;

            await Assert.ThrowsAsync<NullRequestParameterException>(() => _mediator.Send(command));
        }

        [Fact]
        public async Task Mediator_SendNullCommandWhenObject_ShouldThrowsException()
        {
            object command = null;

            await Assert.ThrowsAsync<NullRequestParameterException>(() => _mediator.Send(command));
        }

        [Fact]
        public async Task Mediator_SendEmptyCommand_ShoulApplyValidations()
        {
            var command = new ValidatableCommand();

            var response = await _mediator.Send(command);

            Assert.True(response.HasErrors);
            Assert.Contains(response.Errors, w => w == "'Prop1' não pode ser nulo.");
            Assert.Contains(response.Errors, w => w == "'Prop2' não pode ser nulo.");

            command = new ValidatableCommand()
            {
                Prop1 = "123456",
                Prop2 = "1234567"
            };

            response = await _mediator.Send(command);

            Assert.True(response.HasErrors);
            Assert.Contains(response.Errors, w => w == "'Prop1' deve ser menor ou igual a 5 caracteres. Você digitou 6 caracteres.");
            Assert.Contains(response.Errors, w => w == "'Prop2' deve ser menor ou igual a 6 caracteres. Você digitou 7 caracteres.");

            command = new ValidatableCommand()
            {
                Prop1 = "12345",
                Prop2 = "123456"
            };

            response = await _mediator.Send(command);

            Assert.False(response.HasErrors);
        }
    }

    public class ValidatableCommand : IRequest<ApiResponse>
    {
        public string Prop1 { get; set; }
        public string Prop2 { get; set; }
    }

    public class ValidatableCommandHandler : RequestHandler<ValidatableCommand, ApiResponse>
    {
        protected override ApiResponse Handle(ValidatableCommand request)
        {
            return new ApiResponse();
        }
    }

    public class ValidatableCommandValidator : AbstractValidator<ValidatableCommand>
    {
        public ValidatableCommandValidator()
        {
            RuleFor(r => r.Prop1).NotNull().MaximumLength(5);
            RuleFor(r => r.Prop2).NotNull().MaximumLength(6);
        }
    }

    public class SimpleCommand : IRequest
    {
    }

    public class SimpleCommandHandler : IRequestHandler<SimpleCommand>
    {
        private readonly Counter _counter;

        public SimpleCommandHandler(Counter counter)
        {
            _counter = counter;
        }

        public Task<Unit> Handle(SimpleCommand request, CancellationToken cancellationToken)
        {
            _counter.Increment();

            return Unit.Task;
        }
    }

    public class ResponseCommand : IRequest<ApiResponse>
    {
    }

    public class ResponseCommandHandler : IRequestHandler<ResponseCommand, ApiResponse>
    {
        public Task<ApiResponse> Handle(ResponseCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new ApiResponse().Add("error"));
        }
    }

    public class GenericResponseCommand : IRequest<ApiResponse<User>>
    {
    }

    public class GenericResponseCommandHandler : IRequestHandler<GenericResponseCommand, ApiResponse<User>>
    {
        public Task<ApiResponse<User>> Handle(GenericResponseCommand request, CancellationToken cancellationToken)
        {
            var response = new ApiResponse<User>(new User("user"));

            response.Add("error");

            return Task.FromResult(response);
        }
    }

    public class User
    {
        public string Name { get; set; }

        public User(string name)
        {
            Name = name;
        }
    }

    public class Ping : INotification
    {
    }

    public class Pong1 : INotificationHandler<Ping>
    {
        private readonly Counter _counter;

        public Pong1(Counter counter)
        {
            _counter = counter;
        }

        public Task Handle(Ping notification, CancellationToken cancellationToken)
        {
            _counter.Increment();

            return Task.CompletedTask;
        }
    }

    public class Pong2 : INotificationHandler<Ping>
    {
        private readonly Counter _counter;

        public Pong2(Counter counter)
        {
            _counter = counter;
        }

        public Task Handle(Ping notification, CancellationToken cancellationToken)
        {
            _counter.Increment();

            return Task.CompletedTask;
        }
    }

    public class Counter
    {
        public int Count { get; set; }

        public void Increment()
        {
            Count++;
        }
    }
}
