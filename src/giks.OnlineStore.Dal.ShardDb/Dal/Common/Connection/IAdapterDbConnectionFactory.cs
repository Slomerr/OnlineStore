using System.Data.Common;

namespace giks.OnlineStore.Dal.ShardDb.Dal.Common.Connection;

public interface IAdapterDbConnectionFactory<TShardKey>
{
    DbConnection GetConnectionByShardKey(TShardKey shardKey);
}