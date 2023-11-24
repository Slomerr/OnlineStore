using System.Reflection;
using giks.OnlineStore.CustomerService.Migration;
using giks.OnlineStore.Dal.ShardDb.Dal.Migrations;

public static class Program
{
    public static async Task Main()
    {
        await CreateHostBuilder().Build().RunMigrate();
    }

    private static IHostBuilder CreateHostBuilder()
    {
        return Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(builder => builder
                .ConfigureKestrel(_ => { })
                .UseStartup<Startup>());
    }

    private static async Task RunMigrate(this IHost host)
    {
        using var token = new CancellationTokenSource(TimeSpan.FromMinutes(1));
        using var scope = host.Services.CreateScope();
        var migrator = scope.ServiceProvider.GetRequiredService<IMigrator>();
        await migrator.Migrate(token.Token, Assembly.GetExecutingAssembly());
    }
}