using System.Data;
using System.Data.Common;

namespace giks.OnlineStore.CustomerService.Infrastructure.Dal.Connections;

internal record ConnectionDto(
    DbConnection Connection,
    IDbTransaction Transaction);