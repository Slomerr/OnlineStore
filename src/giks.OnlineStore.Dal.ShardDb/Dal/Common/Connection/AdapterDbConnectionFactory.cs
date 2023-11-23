using System.Data;
using Microsoft.Extensions.Logging;

namespace giks.OnlineStore.Dal.ShardDb.Dal.Common.Connection;

internal sealed class AdapterDbConnectionFactory<TShardKey> : IAdapterDbConnectionFactory<TShardKey>
{
    private readonly IShardingRule<TShardKey> _shardingRule;
    private readonly IShardDbConnectionFactory _connectionFactory;
    private readonly ILogger<AdapterDbConnectionFactory<TShardKey>> _logger;

    public AdapterDbConnectionFactory(
        IShardingRule<TShardKey> shardingRule,
        IShardDbConnectionFactory connectionFactory,
        ILogger<AdapterDbConnectionFactory<TShardKey>> logger)
    {
        _shardingRule = shardingRule;
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public IDbConnection GetConnection(TShardKey shardKey)
    {
        var bucketId = _shardingRule.GetBucketId(shardKey);
        _logger.LogDebug("Get bucket id {bucketId} for shard key {shardKey}", bucketId, shardKey);
        return _connectionFactory.GetConnection(bucketId);
    }
}