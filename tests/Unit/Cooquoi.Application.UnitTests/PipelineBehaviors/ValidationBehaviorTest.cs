using Bogus;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using ValidationException = FluentValidation.ValidationException;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Cooquoi.Application.UnitTests.PipelineBehaviors;

public class ValidationBehaviorTest
{
    public record TestCommand(string Name) : IRequest<string>;

    [Fact]
    public async Task ShouldValidate_BeforeInvokeHandler()
    {
        // Arrange
        var commandInput = new Faker().Lorem.Text();
        var commandOutput = new Faker().Lorem.Text();
        var command = new TestCommand(commandInput);
        
        var validatorMock = Substitute.For<IValidator<TestCommand>>();
        validatorMock
            .ValidateAsync(command, Arg.Any<CancellationToken>())
            .ReturnsForAnyArgs(new ValidationResult());
        
        var handlerMock = Substitute.For<IRequestHandler<TestCommand,string>>();
        handlerMock.Handle(command, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(commandOutput));

        var services = Helper.GetServiceCollection()
            .AddTransient<IValidator<TestCommand>>(_ => validatorMock)
            .AddTransient<IRequestHandler<TestCommand, string>>(_ => handlerMock);

        var provider = services.BuildServiceProvider();
        var mediator = provider.GetRequiredService<IMediator>();
        

        // Act
        var result = await mediator.Send(command);

        // Assert
        await validatorMock
            .Received(1)
            .ValidateAsync(command, Arg.Any<CancellationToken>());
        
        result.Should().Be(commandOutput);
    }
    
    [Fact]
    public async Task ShouldThrowValidationException_BeforeInvokeHandler()
    {
        // Arrange
        var commandInput = new Faker().Lorem.Text();
        var commandOutput = new Faker().Lorem.Text();
        var command = new TestCommand(commandInput);
        
        var validatorMock = Substitute.For<IValidator<TestCommand>>();
        validatorMock
            .ValidateAsync(command, Arg.Any<CancellationToken>())
            .ReturnsForAnyArgs(new ValidationResult(new []{new ValidationFailure("Name", "Name is required")}));
        
        var handlerMock = Substitute.For<IRequestHandler<TestCommand,string>>();
        handlerMock.Handle(command, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(commandOutput));

        var services = Helper.GetServiceCollection()
            .AddTransient<IValidator<TestCommand>>(_ => validatorMock)
            .AddTransient<IRequestHandler<TestCommand, string>>(_ => handlerMock);

        var provider = services.BuildServiceProvider();
        var mediator = provider.GetRequiredService<IMediator>();

        // Act
        var resultTask = async () => await mediator.Send(command);

        // Assert
        await resultTask
            .Should()
            .ThrowAsync<ValidationException>();
    }
}