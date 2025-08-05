using PixelzEcommerce.Modules.Orders.Domain.Events;
using PixelzEcommerce.Shared.Domain;

namespace PixelzEcommerce.Modules.Orders.Domain.Orders;

public sealed class Order : Entity 
{
    private Order(Guid id, string name, OrderStatus status, decimal totalAmount, DateTime createdAt) : base(id)
    {
        Id = id;
        Name = name;
        Status = status;
        TotalAmount = totalAmount;
        CreatedAt = createdAt;
    }

    private Order()
    {
    }
    public static Order Create(string name, OrderStatus status, decimal totalAmount, DateTime createdAt)
    {
        return new Order(Guid.NewGuid(), name, status, totalAmount, createdAt);
    }

    public string Name { get; private set; }
    public OrderStatus Status { get; private set; }
    public decimal TotalAmount { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Result Checkout()
    {
        if (Status != OrderStatus.Created)
        {
            return Result.Failure(OrderErrors.OrderAlreadyCheckedOut);
        }

        Status = OrderStatus.CheckedOut;
        Raise(new OrderCheckoutedDomainEvent(Id));
        return Result.Success();
    }
}
