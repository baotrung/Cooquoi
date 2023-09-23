using Cooquoi.Application.PipelineBehaviors;
using Cooquoi.Core.Functional;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Cooquoi.Application.UnitTests.PipelineBehaviors;

public class ValidationBehaviorTest
{
    internal record TestCommand(string Name) : IRequest<Result<string>>;
    internal class TestCommandValidator : AbstractValidator<TestCommand>
    {
        public TestCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty();
        }
    }
    internal class TestCommandHandler : IRequestHandler<TestCommand, Result<string>>
    {
        public Task<Result<string>> Handle(TestCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(Result<string>.Success(request.Name));
        }
    }

    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("toto", true)]
    public async Task ShouldValidate_BeforeInvokeHandler(string input, bool expected)
    {
        // Arrange
        var services = Helper.GetServiceCollection();
        services.AddTransient<IValidator<TestCommand>, TestCommandValidator>();
        services.AddTransient<IRequestHandler<TestCommand, Result<string>>, TestCommandHandler>();
        services.AddTransient(typeof(IPipelineBehavior<TestCommand, Result<string>>),
            typeof(ValidationBehavior<TestCommand, string>));
        var provider = services.BuildServiceProvider();
        var mediator = provider.GetRequiredService<IMediator>();
        var command = new TestCommand(input);
        
        // Act
        var result = await mediator.Send(command);
        
        // Assert
        result.IsSuccess.Should().Be(expected);
    }
}