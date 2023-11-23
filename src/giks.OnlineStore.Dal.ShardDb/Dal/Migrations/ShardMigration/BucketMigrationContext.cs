using giks.OnlineStore.Dal.ShardDb.Dal.Common;
using giks.OnlineStore.Dal.ShardDb.Dal.Migrations.Exceptions;

namespace giks.OnlineStore.Dal.ShardDb.Dal.Migrations.ShardMigration;

public sealed class BucketMigrationContext
{
    private string _currentSchema = string.Empty;

    public void UpdateCurrentSchema(int bucketId)
    {
        _currentSchema = DbConstants.GetSchemaName(bucketId);
    }

    public string CurrentSchema
    {
        get
        {
            if (string.IsNullOrWhiteSpace(_currentSchema))
            {
                throw new EmptySchemaException();
            }

            return _currentSchema;
        }
    }
}