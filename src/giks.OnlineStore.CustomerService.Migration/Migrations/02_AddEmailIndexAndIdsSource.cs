using FluentMigrator;
using giks.OnlineStore.Dal.ShardDb.Dal.Migrations;
using giks.OnlineStore.Dal.ShardDb.Dal.Migrations.ShardMigration;

namespace giks.OnlineStore.CustomerService.Migration.Migrations;

[Migration(2, "AddEmailIndexAndIdsSource")]
public sealed class AddEmailIndexAndIdsSource : SqlMigration 
{
    protected override string GetUpSql(IServiceProvider serviceProvider)
    {
        var bucketContext = serviceProvider.GetRequiredService<BucketMigrationContext>();
        if (bucketContext.BucketId != 0)
        {
            return String.Empty;
        }

        return @"
create table email_global_t_idx(
    email text,
    customer_id bigint
);

create index email_global_idx 
    on email_global_t_idx(email) 
    include (customer_id);

create table id_source(
    last_index bigint
);

insert into id_source (last_index)
values (0);
";
    }

    protected override string GetDownSql(IServiceProvider serviceProvider)
    {
        var bucketContext = serviceProvider.GetRequiredService<BucketMigrationContext>();
        if (bucketContext.BucketId != 0)
        {
            return String.Empty;
        }

        return @"
drop table id_source;
drop table id_source;
delete index email_global_idx;
drop table email_global_t_idx
";
    }
}