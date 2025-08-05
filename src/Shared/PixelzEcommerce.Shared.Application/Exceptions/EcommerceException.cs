using PixelzEcommerce.Shared.Domain;

namespace PixelzEcommerce.Shared.Application.Exceptions;

public sealed class EcommerceException : Exception
{
    public EcommerceException(string requestName, Error? error = default, Exception? innerException = default)
        : base("Application exception", innerException)
    {
        RequestName = requestName;
        Error = error;
    }

    public string RequestName { get; }

    public Error? Error { get; }
}
