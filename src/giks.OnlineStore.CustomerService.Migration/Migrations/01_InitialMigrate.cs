using FluentMigrator;
using giks.OnlineStore.Dal.ShardDb.Dal.Migrations;

namespace giks.OnlineStore.CustomerService.Migration.Migrations;

[Migration(1, "initial migration")]
public sealed class InitialMigrate : SqlMigration
{
    protected override string GetUpSql(IServiceProvider serviceProvider)
    {
        return @"
create table customer(
    customer_id serial,
    first_name text,
    last_name text,
    email text,
    phone_number text,
    date_creation bigint
);

create index customer_id_idx 
    on customer (customer_id);

";
    }

    protected override string GetDownSql(IServiceProvider serviceProvider)
    {
        return @"
drop table customer; 
";
    }
}