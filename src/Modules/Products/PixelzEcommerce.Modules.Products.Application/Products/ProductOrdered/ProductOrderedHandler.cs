using Microsoft.Extensions.Logging;
using PixelzEcommerce.Shared.Application.Messaging;
using PixelzEcommerce.Shared.Domain;

namespace PixelzEcommerce.Modules.Products.Application.Products.ProductOrdered;
public class ProductOrderedHandler(ILogger<ProductOrderedHandler> logger) : ICommandHandler<ProductOrderedCommand>
{
    public async Task<Result> Handle(ProductOrderedCommand request, CancellationToken cancellationToken)
    {
        await Task.Delay(1000, cancellationToken); // Simulate some processing delay
        logger.LogInformation("Handling ProductOrderedCommand for ProductId: {ProductId}", request.ProductId);
        return Result.Success();
    }
}
