using System.Data;

namespace giks.OnlineStore.Dal.ShardDb.Dal.Common.Connection;

public interface IAdapterDbConnectionFactory<TShardKey>
{
    IDbConnection GetConnection(TShardKey shardKey);
}