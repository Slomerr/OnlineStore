using System.Data.Common;
using giks.OnlineStore.Dal.ShardDb.Dal.Common.Configuration;
using giks.OnlineStore.Dal.ShardDb.Dal.Common.Connection.ShardConnection;
using giks.OnlineStore.Dal.ShardDb.Dal.ServiceDiscovery;
using Npgsql;

namespace giks.OnlineStore.Dal.ShardDb.Dal.Common.Connection;

internal sealed class ShardDbConnectionFactory : IShardDbConnectionFactory
{
    private readonly IDbStore _dbStore;
    private readonly DbConnectionStringBuilder _connectionStringBuilder;
    private readonly SemaphoreSlim _semaphore;
    
    public ShardDbConnectionFactory(
        IDbStore dbStore,
        DbConnectionStringBuilder connectionStringBuilder,
        DbConnectionConfiguration connectionConfiguration)
    {
        _dbStore = dbStore;
        _connectionStringBuilder = connectionStringBuilder;
        _semaphore = new SemaphoreSlim(connectionConfiguration.MaxConnections);
    }

    public DbConnection GetConnection(int bucketId)
    {
        _semaphore.Wait();
        var endpoint = _dbStore.GetEndpoint(bucketId);
        var connectionString = _connectionStringBuilder.BuildConnectionString(endpoint);
        return new ShardDbConnection(
            new NpgsqlConnection(connectionString), 
            bucketId, 
            DisposeConnection);
    }

    private void DisposeConnection()
    {
        _semaphore.Release();
    }
}