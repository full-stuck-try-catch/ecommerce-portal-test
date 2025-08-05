using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PixelzEcommerce.Modules.Orders.Domain.Orders;

namespace PixelzEcommerce.Modules.Orders.Infrastructure.Configurations;
internal sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders");
        builder.HasKey(o => o.Id);

        builder.Property(order => order.Name)
            .HasMaxLength(550)
            .IsRequired();
    }
}
