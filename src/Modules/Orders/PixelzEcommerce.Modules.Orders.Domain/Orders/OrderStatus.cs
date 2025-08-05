namespace PixelzEcommerce.Modules.Orders.Domain.Orders;

public enum OrderStatus
{
    Unknown = 0,
    Created = 1,
    CheckedOut = 2,
    Paid = 3,
    Failed = 4
}
