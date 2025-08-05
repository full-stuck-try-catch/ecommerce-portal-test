using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PixelzEcommerce.Modules.Orders.Application.Orders.CheckoutOrder;
using PixelzEcommerce.Shared.Domain;
using PixelzEcommerce.Shared.Presentation.Endpoints;
using PixelzEcommerce.Shared.Presentation.Results;

namespace PixelzEcommerce.Modules.Orders.Presentation.Orders;

internal sealed class CheckoutOrder : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/orders/{orderId:guid}/checkout", async (ISender sender, Guid orderId) =>
        {
            Result result = await sender.Send(new CheckoutOrderCommand(orderId));

            return result.Match(() => Results.Ok(), ApiResults.Problem);
        })
        .WithTags(Tags.Orders);
    }
}
