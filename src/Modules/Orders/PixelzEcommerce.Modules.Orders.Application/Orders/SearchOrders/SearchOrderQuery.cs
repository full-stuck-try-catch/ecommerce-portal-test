using PixelzEcommerce.Shared.Application.Messaging;

namespace PixelzEcommerce.Modules.Orders.Application.Orders.SearchOrders;
public sealed record SearchOrderQuery(
    string? SearchTerm,
    int? Status,
    string? SortBy,
    string? SortOrder,
    int Page,
    int PageSize
) : IQuery<SearchOrderResponse>;
