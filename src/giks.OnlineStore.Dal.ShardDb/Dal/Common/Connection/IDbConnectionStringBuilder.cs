using giks.OnlineStore.Dal.ShardDb.Dal.ServiceDiscovery;

namespace giks.OnlineStore.Dal.ShardDb.Dal.Common.Connection;

internal interface IDbConnectionStringBuilder
{
    string BuildConnectionString(DbEndpoint endpoint);
}