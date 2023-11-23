namespace giks.OnlineStore.Dal.ShardDb.Dal.ServiceDiscovery;

public record DbEndpoint(
    string HostAndPort,
    int[] BucketIds);