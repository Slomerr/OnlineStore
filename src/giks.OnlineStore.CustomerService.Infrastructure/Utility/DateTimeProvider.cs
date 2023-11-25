using giks.OnlineStore.CustomerService.Application.Utility;

namespace giks.OnlineStore.CustomerService.Infrastructure.Utility;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public long GetDateTimeUtc()
    {
        return ((DateTimeOffset)DateTime.UtcNow).UtcTicks;
    }
}