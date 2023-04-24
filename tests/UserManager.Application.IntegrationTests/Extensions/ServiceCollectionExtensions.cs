using Microsoft.Extensions.DependencyInjection;

namespace UserManager.Application.IntegrationTests.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection Remove<TService>(this IServiceCollection services)
    {
        var serviceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(TService));

        if (serviceDescriptor is not null) services.Remove(serviceDescriptor);

        return services;
    }
}