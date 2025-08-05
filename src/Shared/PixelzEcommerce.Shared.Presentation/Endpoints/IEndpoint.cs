using Microsoft.AspNetCore.Routing;

namespace PixelzEcommerce.Shared.Presentation.Endpoints;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
