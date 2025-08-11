using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using PixelzEcommerce.Shared.Domain;

namespace PixelzEcommerce.Modules.Orders.Infrastructure.DbInterceptors;
public sealed class DomainEventDispatcherInterceptor : SaveChangesInterceptor
{
    private readonly IMediator _mediator;
    public DomainEventDispatcherInterceptor(IMediator mediator)
    {
        _mediator = mediator;
    }
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        DbContext? context = eventData.Context;
        if (context is not null)
        {
            var domainEntities = context.ChangeTracker
           .Entries<Entity>()
           .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any())
           .ToList();

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            // Clear events after saving changes
            domainEntities.ForEach(e => e.Entity.ClearDomainEvents());

            // Dispatch events
            foreach (IDomainEvent? domainEvent in domainEvents)
            {
                await _mediator.Publish(domainEvent, cancellationToken);
            }
        }

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
