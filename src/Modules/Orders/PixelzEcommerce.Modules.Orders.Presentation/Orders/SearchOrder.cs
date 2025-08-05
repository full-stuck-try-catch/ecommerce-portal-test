using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PixelzEcommerce.Modules.Orders.Application.Orders.SearchOrders;
using PixelzEcommerce.Shared.Domain;
using PixelzEcommerce.Shared.Presentation.Endpoints;
using PixelzEcommerce.Shared.Presentation.Results;

namespace PixelzEcommerce.Modules.Orders.Presentation.Orders;

internal sealed class SearchOrder : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders/search", async (ISender sender, string? searchTerm , int? status , string? sortBy, string? sortOrder, int page, int pageSize) =>
        {
           Result<SearchOrderResponse> result = await sender.Send(new SearchOrderQuery(
              searchTerm,
                status,
                sortBy,
                sortOrder,
                page,
                pageSize
            ));

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Orders);
    }
}
