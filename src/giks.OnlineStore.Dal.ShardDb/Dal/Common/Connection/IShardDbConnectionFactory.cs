using System.Data;

namespace giks.OnlineStore.Dal.ShardDb.Dal.Common.Connection;

public interface IShardDbConnectionFactory
{
    IDbConnection GetConnection(int bucketId);
}