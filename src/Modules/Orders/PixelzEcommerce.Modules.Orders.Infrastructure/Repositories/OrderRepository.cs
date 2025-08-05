using Microsoft.EntityFrameworkCore;
using PixelzEcommerce.Modules.Orders.Domain.Orders;
using PixelzEcommerce.Modules.Orders.Infrastructure.Database;

namespace PixelzEcommerce.Modules.Orders.Infrastructure.Repositories;
internal sealed class OrderRepository(OrderDbContext context) : IOrderRepository
{
    public void Add(Order order)
    {
        context.Orders.Add(order);
    }

    public async Task<Order?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Orders.SingleOrDefaultAsync(o => o.Id == id, cancellationToken);
    }
}
