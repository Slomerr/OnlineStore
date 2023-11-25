using System.Data.Common;

namespace giks.OnlineStore.Dal.ShardDb.Dal.Common.Connection;

public interface IShardDbConnectionFactory
{
    DbConnection GetConnection(int bucketId);
}