using System.Data;
using System.Data.Common;
using Npgsql;

namespace giks.OnlineStore.Dal.ShardDb.Dal.Common.Connection.ShardConnection;

internal sealed class ShardDbConnection : DbConnection
{
    public ShardDbConnection(
        NpgsqlConnection connection,
        int bucketId)
    {
        _connection = connection;
        _bucketId = bucketId;
    }

    public override string ConnectionString
    {
        get => _connection.ConnectionString; 
        set => _connection.ConnectionString = value;
    }

    public override string Database => _connection.Database;
    public override ConnectionState State => _connection.State;
    public override string DataSource => _connection.DataSource;
    public override string ServerVersion => _connection.ServerVersion;

    public override void ChangeDatabase(string databaseName) => 
        _connection.ChangeDatabase(databaseName);

    public override void Close() => _connection.Close();

    public override void Open() => _connection.Open();

    protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel) =>
        _connection.BeginTransaction(isolationLevel);

    protected override DbCommand CreateDbCommand()
    {
        var command = _connection.CreateCommand();
        return new ShardDbCommand(command, _bucketId);
    }
    
    private readonly NpgsqlConnection _connection;
    private readonly int _bucketId;
}