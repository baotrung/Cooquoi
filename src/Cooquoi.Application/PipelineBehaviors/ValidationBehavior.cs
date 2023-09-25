using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Cooquoi.Application.PipelineBehaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IServiceProvider _serviceProvider;

    public ValidationBehavior(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var validator = _serviceProvider.GetService<IValidator<TRequest>>();
        if (validator is null)
        {
            return await next();
        }
        
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        
        if (validationResult.IsValid) return await next();
        throw new ValidationException(validationResult.Errors);
    }
}