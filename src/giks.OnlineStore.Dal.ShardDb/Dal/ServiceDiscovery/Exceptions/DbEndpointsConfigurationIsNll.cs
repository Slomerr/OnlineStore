namespace giks.OnlineStore.Dal.ShardDb.Dal.ServiceDiscovery.Exceptions;

public sealed class DbEndpointsConfigurationIsNll : Exception
{
    public DbEndpointsConfigurationIsNll() : base("Db endpoints configuration is empty or incorrect in a configuration files")
    {
    }
}