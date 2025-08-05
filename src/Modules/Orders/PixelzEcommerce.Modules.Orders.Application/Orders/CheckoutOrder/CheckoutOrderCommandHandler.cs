using PixelzEcommerce.Modules.Orders.Application.Abstractions.Data;
using PixelzEcommerce.Modules.Orders.Domain.Orders;
using PixelzEcommerce.Modules.Orders.IntegrationEvents;
using PixelzEcommerce.Shared.Application.EventBus;
using PixelzEcommerce.Shared.Application.Messaging;
using PixelzEcommerce.Shared.Application.TimeUtc;
using PixelzEcommerce.Shared.Domain;

namespace PixelzEcommerce.Modules.Orders.Application.Orders.CheckoutOrder;

internal sealed class CheckoutOrderCommandHandler(
    IEventBus eventBus,
    IDateTimeProvider dateTimeProvider,
    IOrderRepository orderRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<CheckoutOrderCommand>
{
    public async Task<Result> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
    {
        Order? order = await orderRepository.GetAsync(request.OrderId, cancellationToken);

        if (order is null)
        {
            return Result.Failure(OrderErrors.OrderNotFound);
        }

        Result checkoutResult = order.Checkout();

        if (checkoutResult.IsFailure)
        {
            return checkoutResult;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        await eventBus.PublishAsync(new ProductOrderedIntegrationEvent(order.Id, dateTimeProvider.UtcNow), cancellationToken);

        return Result.Success();
    }
}
