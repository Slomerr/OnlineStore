namespace giks.OnlineStore.Dal.ShardDb.Dal.Migrations;

internal record MigrationConfiguration(
    bool IsMigrate,
    long DownToMigrationVersion)
{
    public const string Path = "MigrationConfiguration";
}