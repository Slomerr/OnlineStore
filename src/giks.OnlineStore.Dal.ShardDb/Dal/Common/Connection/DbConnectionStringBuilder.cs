using giks.OnlineStore.Dal.ShardDb.Dal.Common.Configuration;
using giks.OnlineStore.Dal.ShardDb.Dal.ServiceDiscovery;
using Npgsql;

namespace giks.OnlineStore.Dal.ShardDb.Dal.Common.Connection;

internal sealed class DbConnectionStringBuilder : IDbConnectionStringBuilder
{
    private readonly DbConnectionConfiguration _configuration;

    public DbConnectionStringBuilder(DbConnectionConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string BuildConnectionString(DbEndpoint endpoint)
    {
        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = endpoint.HostAndPort,
            Database = _configuration.DbName,
            Username = _configuration.UserId,
            Password = _configuration.Password
        };
        return builder.ToString();
    }
}