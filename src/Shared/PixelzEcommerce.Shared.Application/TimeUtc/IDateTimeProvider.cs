namespace PixelzEcommerce.Shared.Application.TimeUtc;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
