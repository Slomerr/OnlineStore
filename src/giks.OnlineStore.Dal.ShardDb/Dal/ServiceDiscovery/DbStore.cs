using giks.OnlineStore.Dal.ShardDb.Dal.ServiceDiscovery.Exceptions;

namespace giks.OnlineStore.Dal.ShardDb.Dal.ServiceDiscovery;

internal sealed class DbStore : IDbStore
{
    private readonly List<DbEndpoint> _endpoints;

    public DbStore(DbStoreOptions options)
    {
        _endpoints = options.Endpoints.ToList();
        BucketsCount = options.Endpoints.Sum(x => x.BucketIds.Count);
    }

    public int BucketsCount { get; }

    public DbEndpoint GetEndpoint(int bucketId)
    {
        var endpoint = _endpoints.FirstOrDefault(endpoint => endpoint.BucketIds.Contains(bucketId));
        if (endpoint == null)
        {
            throw new NotFoundDbEndpointException(bucketId);
        }

        return endpoint;
    }

    public IReadOnlyCollection<DbEndpoint> GetAllEndpoints() => _endpoints;
}