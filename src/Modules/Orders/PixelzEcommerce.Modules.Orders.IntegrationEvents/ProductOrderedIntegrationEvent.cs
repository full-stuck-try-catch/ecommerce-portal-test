
using PixelzEcommerce.Shared.Application.EventBus;

namespace PixelzEcommerce.Modules.Orders.IntegrationEvents;

public class ProductOrderedIntegrationEvent : IntegrationEvent
{
    public ProductOrderedIntegrationEvent(Guid id, DateTime occurredOnUtc)
       : base(id, occurredOnUtc)
    {
    }

    public Guid ProductId { get; init; }
}
