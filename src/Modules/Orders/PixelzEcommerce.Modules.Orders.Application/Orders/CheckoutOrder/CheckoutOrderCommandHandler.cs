using PixelzEcommerce.Modules.Orders.Application.Abstractions.Data;
using PixelzEcommerce.Modules.Orders.Domain.Orders;
using PixelzEcommerce.Shared.Application.Messaging;
using PixelzEcommerce.Shared.Domain;

namespace PixelzEcommerce.Modules.Orders.Application.Orders.CheckoutOrder;

internal sealed class CheckoutOrderCommandHandler(
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

        Result processPaymentResult = order.StartedCheckout();

        if (processPaymentResult.IsFailure)
        {
            return processPaymentResult;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
