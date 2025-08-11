using MediatR;
using PixelzEcommerce.Modules.Orders.Domain.Events;
using PixelzEcommerce.Modules.Orders.IntegrationEvents;
using PixelzEcommerce.Shared.Application.EventBus;
using PixelzEcommerce.Shared.Application.Messaging;
using PixelzEcommerce.Shared.Application.TimeUtc;

namespace PixelzEcommerce.Modules.Orders.Application.Orders.CheckoutOrder;
public sealed class OrderPaymentProcessingDomainEventHandler(IEventBus eventBus, IDateTimeProvider dateTimeProvider) : DomainEventHandler<OrderPaymentProcessingDomainEvent>
{
    public override async Task Handle(OrderPaymentProcessingDomainEvent notification, CancellationToken cancellationToken = default)
    {
        await eventBus.PublishAsync(new ProductOrderedIntegrationEvent(notification.EventId, dateTimeProvider.UtcNow), cancellationToken);
    }
}
