using giks.OnlineStore.Dal.ShardDb.Dal.Common.Configuration;
using giks.OnlineStore.Dal.ShardDb.Dal.Common.Connection;
using giks.OnlineStore.Dal.ShardDb.Dal.Exceptions;
using giks.OnlineStore.Dal.ShardDb.Dal.Migrations;
using giks.OnlineStore.Dal.ShardDb.Dal.Migrations.ShardMigration;
using giks.OnlineStore.Dal.ShardDb.Dal.ServiceDiscovery;
using giks.OnlineStore.Dal.ShardDb.Dal.ServiceDiscovery.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace giks.OnlineStore.Dal.ShardDb.Dal.Configuration;

public static class DalConfigurationBuilder
{
    public static IServiceCollection AddConnectionFactory(this IServiceCollection services)
    {
        services.AddSingleton<IShardDbConnectionFactory, ShardDbConnectionFactory>();
        return services;
    }

    public static IServiceCollection AddShardingRule<TShardRule, TShardKey>(this IServiceCollection services)
        where TShardRule : IShardingRule<TShardKey>
    {
        services.AddScoped(typeof(IShardingRule<TShardKey>), typeof(TShardRule));
        services.AddScoped<IAdapterDbConnectionFactory<TShardKey>, AdapterDbConnectionFactory<TShardKey>>();
        return services;
    }

    public static IServiceCollection AddDbConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var dbConnection = configuration.GetSection(DbConnectionConfiguration.Path).Get<DbConnectionConfiguration>();
        if (dbConnection == null)
        {
            throw new DbConfigurationIsNull();
        }

        services.AddSingleton<DbConnectionConfiguration>(_ => dbConnection);
        return services;
    }

    public static IServiceCollection AddConnectionBuilder(this IServiceCollection services)
    {
        services.AddSingleton<IDbConnectionStringBuilder, DbConnectionStringBuilder>();
        return services;
    }

    public static IServiceCollection AddShardMigrator(this IServiceCollection services, IConfiguration configuration)
    {
        var migrationConfiguration =
            configuration.GetSection(MigrationConfiguration.Path).Get<MigrationConfiguration>();
        services.AddSingleton<MigrationConfiguration>(_ => migrationConfiguration!);
        services.AddSingleton<IMigrator, ShardMigrator>();
        return services;
    }

    public static IServiceCollection AddDbStore(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection(DbStoreOptions.Path);
        List<DbEndpointConfig> endpoints = new List<DbEndpointConfig>();
        foreach (var sumSection in section.GetChildren())
        {
            endpoints.Add(sumSection.Get<DbEndpointConfig>()!);
        }
        
        services.AddSingleton(_ => new DbStoreOptions(
            endpoints.Select(endpointConfig => endpointConfig.Map()).ToList()));
        services.AddSingleton<IDbStore, DbStore>();
        return services;
    }
}