using System.Data;
using System.Data.Common;
using Npgsql;
using PixelzEcommerce.Shared.Application.Data;

namespace PixelzEcommerce.Shared.Infrastructure.Data;

internal sealed class DbConnectionFactory(NpgsqlDataSource dataSource) : IDbConnectionFactory
{
    public async ValueTask<DbConnection> OpenConnectionAsync()
    {
        return await dataSource.OpenConnectionAsync();
    }

    public IDbConnection CreateConnection()
    {
        return dataSource.CreateConnection();
    }
}
