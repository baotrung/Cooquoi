using Cooquoi.Core.Functional;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Cooquoi.Application.PipelineBehaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, Result<TResponse>>
    where TRequest : IRequest<Result<TResponse>>
{
    private readonly IServiceProvider _serviceProvider;

    public ValidationBehavior(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<Result<TResponse>> Handle(
        TRequest request, 
        RequestHandlerDelegate<Result<TResponse>> next,
        CancellationToken cancellationToken)
    {
        var validator = _serviceProvider.GetService<IValidator<TRequest>>();
        if (validator is null)
        {
            return await next();
        }
        
        var context = new ValidationContext<TRequest>(request);
        
        var validationResult = await validator.ValidateAsync(context, cancellationToken);
        
        if (validationResult.IsValid) return await next();
        var failures = validationResult
            .Errors
            .Select(err => new Failure
            {
                Code = "ValidationError",
                Message = err.ErrorMessage,
                Data = err
            })
            .ToList();
        return Result<TResponse>.Fail(failures);
    }
}