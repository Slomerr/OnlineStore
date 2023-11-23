namespace giks.OnlineStore.Dal.ShardDb.Dal.Migrations.Exceptions;

public sealed class EmptySchemaException : Exception
{
    public EmptySchemaException() : base("Current schema in bucket context is empty")
    {
    }
}