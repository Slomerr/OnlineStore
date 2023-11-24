namespace giks.OnlineStore.Dal.ShardDb.Dal.ServiceDiscovery;

internal static class DbEndpointExtensions
{
    public static DbEndpoint Map(this DbEndpointConfig endpointConfig)
    {
        return new DbEndpoint(endpointConfig.HostAndPort, endpointConfig.BucketIds);
    }
}