using Microsoft.EntityFrameworkCore;
using PixelzEcommerce.Modules.Orders.Application.Abstractions.Data;
using PixelzEcommerce.Modules.Orders.Domain.Orders;
using PixelzEcommerce.Modules.Orders.Infrastructure.Configurations;

namespace PixelzEcommerce.Modules.Orders.Infrastructure.Database;

public sealed class OrderDbContext(DbContextOptions<OrderDbContext> options)
    : DbContext(options), IUnitOfWork
{
    internal DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.Order);

        modelBuilder.ApplyConfiguration(new OrderConfiguration());
    }
}
