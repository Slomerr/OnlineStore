using FluentMigrator.Runner.VersionTableInfo;

namespace giks.OnlineStore.Dal.ShardDb.Dal.Migrations.ShardMigration;

public sealed class VersionTableMetaData : IVersionTableMetaData
{
    private readonly BucketMigrationContext _migrationContext;

    public VersionTableMetaData(BucketMigrationContext migrationContext)
    {
        _migrationContext = migrationContext;
    }

    public object ApplicationContext { get; set; } = null!;
    public bool OwnsSchema => true;
    public string SchemaName => _migrationContext.CurrentSchema;
    public string TableName => "version_info";
    public string ColumnName => "version";
    public string DescriptionColumnName => "description";
    public string UniqueIndexName => "version_unq_idx";
    public string AppliedOnColumnName => "applied_on";
}