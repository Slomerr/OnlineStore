namespace giks.OnlineStore.Dal.ShardDb.Dal.ServiceDiscovery;

public interface IDbStore
{
    int BucketsCount { get; }
    DbEndpoint GetEndpoint(int bucketId);
    IReadOnlyCollection<DbEndpoint> GetAllEndpoints();
}