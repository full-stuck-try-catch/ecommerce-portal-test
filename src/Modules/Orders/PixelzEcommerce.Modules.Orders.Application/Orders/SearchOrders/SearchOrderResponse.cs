using System.Collections.Generic;

namespace PixelzEcommerce.Modules.Orders.Application.Orders.SearchOrders;


public record SearchOrderResponse(
    int Page,
    int PageSize,
    int TotalCount,
    IReadOnlyCollection<OrderResponse> Orders
);

public sealed record OrderResponse(
    Guid Id,
    string Name,
    string Status,
    decimal TotalAmount,
    DateTime CreatedAt
);
