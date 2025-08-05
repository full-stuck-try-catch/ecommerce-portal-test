using MediatR;
using PixelzEcommerce.Shared.Domain;

namespace PixelzEcommerce.Shared.Application.Messaging;

public interface ICommand : IRequest<Result>, IBaseCommand;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand;

public interface IBaseCommand;
