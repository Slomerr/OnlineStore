using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using Npgsql;

namespace giks.OnlineStore.Dal.ShardDb.Dal.Common.Connection.ShardConnection;

internal sealed class ShardDbCommand : DbCommand
{
    public ShardDbCommand(
        NpgsqlCommand command,
        int bucketId)
    {
        _command = command;
        _bucketId = bucketId;
    }

    [AllowNull]
    public override string CommandText
    {
        get =>_command.CommandText;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                _command.CommandText = null;
                return;
            }

            var bucketName = DbConstants.GetSchemaName(_bucketId);
            var sql = value.Replace(DbConstants.BucketPlaceholder, bucketName);
            _command.CommandText = sql;
        }
    }

    public override int CommandTimeout
    {
        get => _command.CommandTimeout; 
        set => _command.CommandTimeout = value;
    }

    public override CommandType CommandType
    {
        get => _command.CommandType; 
        set => _command.CommandType = value;
    }

    public override UpdateRowSource UpdatedRowSource
    {
        get => _command.UpdatedRowSource; 
        set => _command.UpdatedRowSource = value;
    }

    protected override DbConnection? DbConnection
    {
        get => _command.Connection;
        set => _command.Connection = value as NpgsqlConnection;
    }

    protected override DbParameterCollection DbParameterCollection => _command.Parameters;

    protected override DbTransaction? DbTransaction
    {
        get => _command.Transaction; 
        set => _command.Transaction = value as NpgsqlTransaction;
    }

    public override bool DesignTimeVisible
    {
        get => _command.DesignTimeVisible; 
        set => _command.DesignTimeVisible = value;
    }


    public override void Cancel() => _command.Cancel();

    public override int ExecuteNonQuery() => _command.ExecuteNonQuery();

    public override object? ExecuteScalar() => _command.ExecuteScalar();

    public override void Prepare() => _command.Prepare();
    protected override DbParameter CreateDbParameter() => _command.CreateParameter();

    protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior) =>
        _command.ExecuteReader(behavior);

    public override ValueTask DisposeAsync()
    {
        _command.DisposeAsync();
        return base.DisposeAsync();
    }

    protected override void Dispose(bool disposing)
    {
        _command.Dispose();
        base.Dispose(disposing);
    }

    private readonly NpgsqlCommand _command;
    private readonly int _bucketId;
}