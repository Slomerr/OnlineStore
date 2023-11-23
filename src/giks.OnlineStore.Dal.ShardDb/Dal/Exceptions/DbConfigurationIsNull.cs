namespace giks.OnlineStore.Dal.ShardDb.Dal.Exceptions;

public sealed class DbConfigurationIsNull : Exception
{
    public DbConfigurationIsNull() : base("Db connection configuration is empty or incorrect in a configuration files")
    {
    }
}