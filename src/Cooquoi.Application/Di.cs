using Cooquoi.Application.PipelineBehaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Cooquoi.Application;

public static class Di
{
    public static IServiceCollection AddCooquoiApplication(this IServiceCollection services)
    {
        var assembly = typeof(Di).Assembly;
        
        // fluent validation
        services.AddValidatorsFromAssembly(assembly);
        
        // mediatr
        services.AddMediatR(cf =>
        {
            cf.RegisterServicesFromAssembly(assembly);
            cf.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
        
        return services;
    }
}