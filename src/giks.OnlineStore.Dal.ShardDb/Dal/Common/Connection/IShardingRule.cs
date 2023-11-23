namespace giks.OnlineStore.Dal.ShardDb.Dal.Common.Connection;

public interface IShardingRule<TShardKey>
{
    int GetBucketId(TShardKey shardKey);
}