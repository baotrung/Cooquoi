using Cooquoi.Application.Interfaces;
using Cooquoi.Application.UseCases.Storage.Commands;
using Cooquoi.Core.Functional;
using Cooquoi.Domain.Entities;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Cooquoi.Application.UnitTests;

public class UnitTest1
{
    [Fact]
    public async Task Test1()
    {
        // Arrange
        var services = Helper.GetServiceCollection();
        var storageRepositoryMock = Substitute.For<IStorageRepository>();
        storageRepositoryMock.AddNewStorage(Arg.Any<Storage>()).ReturnsForAnyArgs(Result.Success());
        services.AddTransient<IStorageRepository>(_ => storageRepositoryMock);
        var provider = services.BuildServiceProvider();
        var mediator = provider.GetRequiredService<IMediator>();
        var command = new CreateStorage.Command(string.Empty);
        
        // Act
        var result = await mediator.Send(command);
        
        // Assert
        result.IsSuccess.Should().BeFalse();
    }
}