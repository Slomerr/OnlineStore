using giks.OnlineStore.Dal.ShardDb.Dal.Common.Connection.Rules;
using giks.OnlineStore.Dal.ShardDb.Dal.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace giks.OnlineStore.CustomerService.Infrastructure.Dal;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDal(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddShardingRule<LongShardingRule, long>()
            .AddConnectionFactory()
            .AddDbConfiguration(configuration)
            .AddDbStore(configuration)
            .AddConnectionBuilder();
        return services;
    }
}