using System.Data.Common;
using Dapper;
using PixelzEcommerce.Modules.Orders.Domain.Orders;
using PixelzEcommerce.Shared.Application.Data;
using PixelzEcommerce.Shared.Application.Messaging;
using PixelzEcommerce.Shared.Domain;

namespace PixelzEcommerce.Modules.Orders.Application.Orders.SearchOrders;

internal sealed class SearchOrderQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<SearchOrderQuery, SearchOrderResponse>
{
    private const string SchemaTable = @"""order"".orders";

    private static readonly Dictionary<string, string> ValidSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        ["name"] = "o.name",
        ["status"] = "o.status",
        ["totalamount"] = "o.total_amount",
        ["createdat"] = "o.created_at"
    };

    private const string DefaultSortColumn = "o.created_at";

    public async Task<Result<SearchOrderResponse>> Handle(SearchOrderQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        (string whereClause, DynamicParameters parameters) = BuildWhere(request);

        // Count
        string countSql = $"""
            SELECT COUNT(*)
            FROM {SchemaTable} o
            {whereClause}
            """;

        var countCmd = new CommandDefinition(countSql, parameters, cancellationToken: cancellationToken);
        int totalCount = await connection.ExecuteScalarAsync<int>(countCmd);

        // Sorting + paging
        string orderByClause = BuildOrderBy(request.SortBy, request.SortOrder);
        (int offset, int limit) = NormalizePaging(request.Page, request.PageSize);

        parameters.Add("Offset", offset);
        parameters.Add("Limit", limit);

        // Main query (project raw values and map status in C#)
        string selectSql = $"""
            SELECT
                o.id            AS {nameof(OrderRow.Id)},
                o.name          AS {nameof(OrderRow.Name)},
                o.status        AS {nameof(OrderRow.Status)},          -- int in DB
                o.total_amount  AS {nameof(OrderRow.TotalAmount)},
                o.created_at    AS {nameof(OrderRow.CreatedAt)}
            FROM {SchemaTable} o
            {whereClause}
            {orderByClause}
            OFFSET @Offset
            LIMIT  @Limit
            """;

        var selectCmd = new CommandDefinition(selectSql, parameters, cancellationToken: cancellationToken);
        IEnumerable<OrderRow> rows = await connection.QueryAsync<OrderRow>(selectCmd);

        var orders = rows.Select(r => new OrderResponse(
            Id: r.Id,
            Name: r.Name,
            Status: MapStatusName(r.Status),
            TotalAmount: r.TotalAmount,
            CreatedAt: r.CreatedAt
        )).ToList();

        var response = new SearchOrderResponse(
            Page: request.Page,
            PageSize: request.PageSize,
            TotalCount: totalCount,
            Orders: orders
        );

        return Result.Success(response);
    }

    private static (string whereClause, DynamicParameters parameters) BuildWhere(SearchOrderQuery request)
    {
        var conditions = new List<string>();
        var p = new DynamicParameters();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            conditions.Add("o.name ILIKE @SearchTerm");
            p.Add("SearchTerm", $"%{request.SearchTerm}%");
        }

        if (request.Status.HasValue)
        {
            conditions.Add("o.status = @Status");
            p.Add("Status", request.Status.Value);
        }

        string where = conditions.Count > 0 ? "WHERE " + string.Join(" AND ", conditions) : string.Empty;
        return (where, p);
    }

    private static string BuildOrderBy(string? sortBy, string? sortOrder)
    {
        string column = DefaultSortColumn;
        if (!string.IsNullOrWhiteSpace(sortBy) && ValidSortColumns.TryGetValue(sortBy, out string? mapped))
        {
            column = mapped;
        }

        // Default DESC; only explicit "asc" makes it ascending
        string direction = string.Equals(sortOrder, "asc", StringComparison.OrdinalIgnoreCase) ? "ASC" : "DESC";
        return $"ORDER BY {column} {direction}";
    }

    private static (int offset, int limit) NormalizePaging(int page, int pageSize)
    {
        int p = Math.Max(1, page);
        int size = Math.Clamp(pageSize, 1, 200);
        int offset = (p - 1) * size;
        return (offset, size);
    }

    private static string MapStatusName(int statusValue)
    {
        // If DB has unknown future values, this is resilient.
        return Enum.IsDefined(typeof(OrderStatus), statusValue)
            ? ((OrderStatus)statusValue).ToString()
            : "Unknown";
    }

    // Local DTO purely for Dapper row mapping
    private sealed record OrderRow(
        Guid Id,
        string Name,
        int Status,
        decimal TotalAmount,
        DateTime CreatedAt
    );
}
