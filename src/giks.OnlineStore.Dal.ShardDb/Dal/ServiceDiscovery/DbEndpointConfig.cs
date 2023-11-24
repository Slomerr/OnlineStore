namespace giks.OnlineStore.Dal.ShardDb.Dal.ServiceDiscovery;

internal sealed class DbEndpointConfig
{
    public string HostAndPort { get; set; }
    public IReadOnlyCollection<int> BucketIds { get; set; }
}
