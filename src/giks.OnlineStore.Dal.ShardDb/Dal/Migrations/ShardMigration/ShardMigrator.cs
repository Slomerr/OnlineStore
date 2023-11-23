using System.Reflection;
using FluentMigrator.Runner;
using giks.OnlineStore.Dal.ShardDb.Dal.Common.Connection;
using giks.OnlineStore.Dal.ShardDb.Dal.ServiceDiscovery;
using Microsoft.Extensions.DependencyInjection;

namespace giks.OnlineStore.Dal.ShardDb.Dal.Migrations.ShardMigration;

internal sealed class ShardMigrator : IMigrator
{
    private readonly IDbConnectionStringBuilder _connectionBuilder;
    private readonly IDbStore _dbStore;

    public ShardMigrator(
        IDbConnectionStringBuilder connectionBuilder,
        IDbStore dbStore)
    {
        _connectionBuilder = connectionBuilder;
        _dbStore = dbStore;
    }
    
    public Task Migrate(CancellationToken token, Assembly assemblyWithMigrations)
    {
        var endpoints = _dbStore.GetAllEndpoints();
        foreach (var endpoint in endpoints)
        {
            var connectionString = _connectionBuilder.BuildConnectionString(endpoint);
            foreach (var bucketId in endpoint.BucketIds)
            {
                var serviceProvider = CreateService(connectionString, assemblyWithMigrations);
                using var scope = serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<BucketMigrationContext>();
                context.UpdateCurrentSchema(bucketId);
                var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
                runner.MigrateUp();
            }
        }
        
        return Task.CompletedTask;
    }

    private IServiceProvider CreateService(string connectionString, Assembly assemblyWithMigrations)
    {
        return new ServiceCollection()
            .AddSingleton<BucketMigrationContext>()
            .AddFluentMigratorCore()
            .ConfigureRunner(builder => builder
                .AddPostgres()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(assemblyWithMigrations).For.Migrations()
                .ScanIn(typeof(VersionTableMetaData).Assembly).For.VersionTableMetaData())
            .BuildServiceProvider();
    }
}