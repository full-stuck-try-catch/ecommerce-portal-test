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
    private static readonly Dictionary<string, string> ValidSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        { "name", "o.name" },
        { "status", "o.status" },
        { "totalamount", "o.total_amount" },
        { "createdat", "o.created_at" }
    };

    private const string DefaultSortColumn = "o.created_at";
    private const string AscendingOrder = "asc";
    private const string DescendingSort = "DESC";
    private const string AscendingSort = "ASC";

    public async Task<Result<SearchOrderResponse>> Handle(SearchOrderQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        var whereConditions = new List<string>();
        var parameters = new DynamicParameters();

        // Build search conditions
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            whereConditions.Add("o.name ILIKE @SearchTerm");
            parameters.Add("SearchTerm", $"%{request.SearchTerm}%");
        }

        if (request.Status.HasValue)
        {
            whereConditions.Add("o.status = @Status");
            parameters.Add("Status", request.Status.Value);
        }

        string whereClause = whereConditions.Count > 0
            ? "WHERE " + string.Join(" AND ", whereConditions)
            : string.Empty;

        // Get total count for pagination
        string countSql = $@"
            SELECT COUNT(*)
            FROM ""order"".orders o
            {whereClause}";

        int totalCount = await connection.QuerySingleAsync<int>(countSql, parameters);

        string orderByClause = BuildOrderByClause(request.SortBy, request.SortOrder);

        int offset = (request.Page - 1) * request.PageSize;
        parameters.Add("Offset", offset);
        parameters.Add("PageSize", request.PageSize);

        // Build the main query
        string sql = $@"
            SELECT 
                o.id AS {nameof(OrderResponse.Id)},
                o.name AS {nameof(OrderResponse.Name)},
                CASE 
                    WHEN o.status = 1 THEN '{nameof(OrderStatus.Created)}'
                    WHEN o.status = 2 THEN '{nameof(OrderStatus.CheckedOut)}'
                    WHEN o.status = 3 THEN '{nameof(OrderStatus.Paid)}'
                    WHEN o.status = 4 THEN '{nameof(OrderStatus.Failed)}'
                    ELSE 'Unknown'
                END AS {nameof(OrderResponse.Status)},
                o.total_amount AS {nameof(OrderResponse.TotalAmount)},
                o.created_at AS {nameof(OrderResponse.CreatedAt)}
            FROM ""order"".orders o
            {whereClause}
            {orderByClause}
            OFFSET @Offset ROWS
            FETCH NEXT @PageSize ROWS ONLY";

        IEnumerable<OrderResponse> orders = await connection.QueryAsync<OrderResponse>(sql, parameters);

        var response = new SearchOrderResponse(
            Page: request.Page,
            PageSize: request.PageSize,
            TotalCount: totalCount,
            Orders: orders.ToList()
        );

        return Result.Success(response);
    }

    private static string BuildOrderByClause(string? sortBy, string? sortOrder)
    {
        string column = DefaultSortColumn;

        if (!string.IsNullOrWhiteSpace(sortBy) && ValidSortColumns.TryGetValue(sortBy, out string? mappedColumn))
        {
            column = mappedColumn;
        }

        string direction = string.Equals(sortOrder, AscendingOrder, StringComparison.OrdinalIgnoreCase) 
            ? AscendingSort 
            : DescendingSort;

        return $"ORDER BY {column} {direction}";
    }
}
