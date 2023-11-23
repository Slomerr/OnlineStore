using FluentMigrator;
using FluentMigrator.Expressions;
using FluentMigrator.Infrastructure;
using giks.OnlineStore.Dal.ShardDb.Dal.Migrations.ShardMigration;
using Microsoft.Extensions.DependencyInjection;

namespace giks.OnlineStore.Dal.ShardDb.Dal.Migrations;

public abstract class SqlMigration : IMigration
{
    public void GetUpExpressions(IMigrationContext context)
    {
        var bucketContext = context.ServiceProvider.GetRequiredService<BucketMigrationContext>();
        var schema = bucketContext.CurrentSchema;
        if (!context.QuerySchema.SchemaExists(schema))
        {
            context.Expressions.Add(new ExecuteSqlStatementExpression{ SqlStatement = $"create schema {schema}"});
        }
        
        context.Expressions.Add(new ExecuteSqlStatementExpression { SqlStatement = $"set search_path to {schema}"});
        var sqlStatement = GetUpSql(context.ServiceProvider);
        context.Expressions.Add(new ExecuteSqlStatementExpression{SqlStatement = sqlStatement});
    }

    public void GetDownExpressions(IMigrationContext context)
    {
        context.Expressions.Add(new ExecuteSqlStatementExpression{SqlStatement = GetDownSql(context.ServiceProvider)});
    }

    public object ApplicationContext { get; } = null!;
    public string ConnectionString { get; } = null!;

    protected abstract string GetUpSql(IServiceProvider serviceProvider);
    protected abstract string GetDownSql(IServiceProvider serviceProvider);
}