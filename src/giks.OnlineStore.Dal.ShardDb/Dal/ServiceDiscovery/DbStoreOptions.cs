namespace giks.OnlineStore.Dal.ShardDb.Dal.ServiceDiscovery;

public record DbStoreOptions(
    IReadOnlyCollection<DbEndpoint> Endpoints);