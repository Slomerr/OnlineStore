using giks.OnlineStore.Dal.ShardDb.Dal.Configuration;

namespace giks.OnlineStore.CustomerService.Migration;

internal sealed class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbConfiguration(_configuration)
            .AddDbStore(_configuration)
            .AddConnectionBuilder()
            .AddShardMigrator();
    }

    public void Configure(IApplicationBuilder app)
    {
        
    }
}