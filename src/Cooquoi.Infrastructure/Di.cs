using Cooquoi.Application.Interfaces;
using Cooquoi.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Cooquoi.Infrastructure;

public static class Di
{
    public static IServiceCollection AddCooquoiInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IStorageRepository, InMemoryStorageRepository>();
        services.AddSingleton<IIngredientRepository, InMemoryIngredientRepository>();

        return services;
    }
}