using System.Reflection;

namespace giks.OnlineStore.Dal.ShardDb.Dal.Migrations;

public interface IMigrator
{
    Task Migrate(CancellationToken token, Assembly assemblyWithMigrations);
}