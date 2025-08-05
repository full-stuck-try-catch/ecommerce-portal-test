using PixelzEcommerce.Shared.Application.Messaging;

namespace PixelzEcommerce.Modules.Products.Application.Products.ProductOrdered;
public sealed record ProductOrderedCommand(Guid ProductId) : ICommand;
