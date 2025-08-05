using PixelzEcommerce.Shared.Application.Email;

namespace PixelzEcommerce.Shared.Infrastructure.Email;

internal sealed class EmailService : IEmailService
{
    public Task SendAsync(string recipient, string subject, string body)
    {
        return Task.CompletedTask;
    }
}
