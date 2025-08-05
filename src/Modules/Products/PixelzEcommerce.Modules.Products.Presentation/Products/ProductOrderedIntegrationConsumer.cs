using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using MediatR;
using PixelzEcommerce.Modules.Orders.IntegrationEvents;
using PixelzEcommerce.Modules.Products.Application.Products.ProductOrdered;
using PixelzEcommerce.Shared.Application.Exceptions;
using PixelzEcommerce.Shared.Domain;

namespace PixelzEcommerce.Modules.Products.Presentation.Products;

public sealed class ProductOrderedIntegrationConsumer(ISender sender) : IConsumer<ProductOrderedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductOrderedIntegrationEvent> context)
    {
        Result result = await sender.Send(
           new ProductOrderedCommand(
               context.Message.ProductId));

        if (result.IsFailure)
        {
            throw new EcommerceException(nameof(ProductOrderedCommand), result.Error);
        }
    }
}
