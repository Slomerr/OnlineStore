namespace giks.OnlineStore.Dal.ShardDb.Dal.Common;

internal static class DbConstants
{
    public const string BucketPlaceholder = "__bucket__";
    public const string BucketSchemaNameFormat = "bucket_{0}";

    public static string GetSchemaName(int bucketId) => string.Format(BucketSchemaNameFormat, bucketId);
}