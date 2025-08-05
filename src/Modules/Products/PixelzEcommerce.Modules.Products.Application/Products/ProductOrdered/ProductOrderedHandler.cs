using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PixelzEcommerce.Shared.Application.Messaging;
using PixelzEcommerce.Shared.Domain;

namespace PixelzEcommerce.Modules.Products.Application.Products.ProductOrdered;
public class ProductOrderedHandler(ILogger<ProductOrderedHandler> logger) : ICommandHandler<ProductOrderedCommand>
{
    public async Task<Result> Handle(ProductOrderedCommand request, CancellationToken cancellationToken)
    {
        await Task.Delay(1000, cancellationToken);

        logger.LogInformation(
               "Product Ordered Success: {ProductId}}",
               request.ProductId);

        return Result.Success();
    }
}
