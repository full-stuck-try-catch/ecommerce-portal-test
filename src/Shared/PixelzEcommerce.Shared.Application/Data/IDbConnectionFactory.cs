using System.Data;
using System.Data.Common;

namespace PixelzEcommerce.Shared.Application.Data;

public interface IDbConnectionFactory
{
    ValueTask<DbConnection> OpenConnectionAsync();
    IDbConnection CreateConnection();
}
