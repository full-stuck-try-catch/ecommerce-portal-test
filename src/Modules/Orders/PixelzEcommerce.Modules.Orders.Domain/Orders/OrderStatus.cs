namespace PixelzEcommerce.Modules.Orders.Domain.Orders;

public enum OrderStatus
{
    Unknown = 0,
    Created = 1,
    PendingPayment = 2,
    Failed = 3,
    Confirmed = 4,
    Completed = 5,
    Canceled = 6,
    Refunded = 7
}
