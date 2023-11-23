namespace giks.OnlineStore.Dal.ShardDb.Dal.Common.Configuration;

public record DbConnectionConfiguration(
    string DbName,
    string UserId,
    string Password);