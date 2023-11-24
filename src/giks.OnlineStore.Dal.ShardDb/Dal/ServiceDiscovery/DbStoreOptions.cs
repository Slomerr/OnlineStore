namespace giks.OnlineStore.Dal.ShardDb.Dal.ServiceDiscovery;

public record DbStoreOptions(
    IReadOnlyCollection<DbEndpoint> Endpoints
    )
{
    public const string Path = "Endpoints";
}