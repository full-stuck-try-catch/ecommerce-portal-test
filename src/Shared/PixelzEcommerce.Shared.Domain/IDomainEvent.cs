using MediatR;

namespace PixelzEcommerce.Shared.Domain;

public interface IDomainEvent
{
    Guid Id { get; }

    DateTime OccurredOnUtc { get; }
}
