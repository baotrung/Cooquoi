using Microsoft.Extensions.DependencyInjection;

namespace Cooquoi.Application.UnitTests;

public static class Helper
{
    public static IServiceCollection GetServiceCollection()
    {
        var services = new ServiceCollection();
        services.AddCooquoiApplication();
        return services;
    }
}