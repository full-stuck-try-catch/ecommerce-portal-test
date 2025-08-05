using MediatR;
using PixelzEcommerce.Shared.Domain;

namespace PixelzEcommerce.Shared.Application.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
