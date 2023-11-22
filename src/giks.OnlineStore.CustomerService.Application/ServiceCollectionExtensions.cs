using giks.OnlineStore.CustomerService.Application.Customers;
using Microsoft.Extensions.DependencyInjection;

namespace giks.OnlineStore.CustomerService.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomerService(this IServiceCollection services)
    {
        services.AddScoped<ICustomerService, Customers.CustomerService>();
        
        return services;
    }
}