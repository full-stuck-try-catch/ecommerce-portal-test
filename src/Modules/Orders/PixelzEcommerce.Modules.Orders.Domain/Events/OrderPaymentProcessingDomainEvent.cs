using PixelzEcommerce.Shared.Domain;

namespace PixelzEcommerce.Modules.Orders.Domain.Events;

public sealed class OrderPaymentProcessingDomainEvent(Guid orderId) : DomainEvent
{
    public Guid EventId { get; init; } = orderId;
}
