using Cooquoi.Application.PipelineBehaviors;
using Cooquoi.Core.Functional;
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
        });

        foreach (var type in assembly.GetTypes())
        {
            if (type is not {IsAbstract: false, IsGenericTypeDefinition: false}) continue;
            foreach (var typeInterface in type.GetInterfaces())
            {
                if (!typeInterface.IsGenericType || typeInterface.GetGenericTypeDefinition() != typeof(IRequestHandler<,>))
                    continue;
                var requestType = typeInterface.GetGenericArguments()[0];
                var responseType = typeInterface.GetGenericArguments()[1];

                if (responseType.GetGenericTypeDefinition() != typeof(Result<>)) continue;
                var dataType = responseType.GetGenericArguments()[0];
                services.AddTransient(
                    typeof(IPipelineBehavior<,>).MakeGenericType(requestType, responseType), 
                    typeof(ValidationBehavior<,>).MakeGenericType(requestType, dataType));
            }
        }
        
        return services;
    }
}