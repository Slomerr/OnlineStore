using System.Data.Common;
using giks.OnlineStore.Dal.ShardDb.Dal.Common.Connection.ShardConnection;
using giks.OnlineStore.Dal.ShardDb.Dal.ServiceDiscovery;
using Npgsql;

namespace giks.OnlineStore.Dal.ShardDb.Dal.Common.Connection;

internal sealed class ShardDbConnectionFactory : IShardDbConnectionFactory
{
    private readonly IDbStore _dbStore;
    private readonly DbConnectionStringBuilder _connectionStringBuilder;

    public ShardDbConnectionFactory(
        IDbStore dbStore,
        DbConnectionStringBuilder connectionStringBuilder)
    {
        _dbStore = dbStore;
        _connectionStringBuilder = connectionStringBuilder;
    }

    public DbConnection GetConnection(int bucketId)
    {
        var endpoint = _dbStore.GetEndpoint(bucketId);
        var connectionString = _connectionStringBuilder.BuildConnectionString(endpoint);
        return new ShardDbConnection(new NpgsqlConnection(connectionString), bucketId);
    }
}