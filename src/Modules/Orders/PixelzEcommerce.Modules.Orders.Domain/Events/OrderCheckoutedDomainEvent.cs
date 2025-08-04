using PixelzEcommerce.Shared.Domain;

namespace PixelzEcommerce.Modules.Orders.Domain.Events;

public sealed class OrderCheckoutedDomainEvent(Guid eventId) : IDomainEvent
{
    public Guid EventId { get; init; } = eventId;
}
