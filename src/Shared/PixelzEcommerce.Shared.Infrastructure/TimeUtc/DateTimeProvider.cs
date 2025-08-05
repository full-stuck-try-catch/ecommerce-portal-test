using PixelzEcommerce.Shared.Application.TimeUtc;

namespace PixelzEcommerce.Shared.Infrastructure.TimeUtc;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
