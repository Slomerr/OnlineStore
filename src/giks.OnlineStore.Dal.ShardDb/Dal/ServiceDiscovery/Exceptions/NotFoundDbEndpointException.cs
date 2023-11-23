namespace giks.OnlineStore.Dal.ShardDb.Dal.ServiceDiscovery.Exceptions;

public sealed class NotFoundDbEndpointException : Exception
{
    public NotFoundDbEndpointException(int bucketId) : base($"Not found db endpoint for bucket id={bucketId}")
    {
    }
}