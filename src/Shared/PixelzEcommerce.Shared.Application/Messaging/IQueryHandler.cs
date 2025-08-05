using MediatR;
using PixelzEcommerce.Shared.Domain;

namespace PixelzEcommerce.Shared.Application.Messaging;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>;
