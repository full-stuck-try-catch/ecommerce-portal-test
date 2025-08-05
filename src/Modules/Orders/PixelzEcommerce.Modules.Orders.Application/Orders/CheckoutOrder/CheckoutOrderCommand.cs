using PixelzEcommerce.Shared.Application.Messaging;

namespace PixelzEcommerce.Modules.Orders.Application.Orders.CheckoutOrder;

public sealed record CheckoutOrderCommand(Guid OrderId) : ICommand;
