using giks.OnlineStore.Dal.ShardDb.Dal.ServiceDiscovery;
using Murmur;

namespace giks.OnlineStore.Dal.ShardDb.Dal.Common.Connection.Rules;

public sealed class LongShardingRule : IShardingRule<long>
{
    private readonly IDbStore _dbStore;

    public LongShardingRule(IDbStore dbStore)
    {
        _dbStore = dbStore;
    }
    
    public int GetBucketId(long shardKey)
    {
        var hash = GetHashCodeByShardKey(shardKey);
        return Math.Abs(hash) % _dbStore.BucketsCount;
    }

    private int GetHashCodeByShardKey(long shardKey)
    {
        var bytes = BitConverter.GetBytes(shardKey);
        var hasher = MurmurHash.Create32();
        var hash = hasher.ComputeHash(bytes);
        return BitConverter.ToInt32(hash);
    } 
}